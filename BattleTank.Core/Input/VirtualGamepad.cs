using System;
using BattleTank.Core.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SlimDX.Direct3D10;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace BattleTank.Core.Input
{
    class VirtualGamepad : ITankActionProvider
    {
        private Button _mineButton;
        private Button _fireButton;
        private Texture2D _joystickTop;
        private Texture2D _joystickBase;
        private Vector2 _xyJoyMove;
        
        public Texture2D JoystickBase
        {
            get { return _joystickBase; }
            set { _joystickBase = value; }
        }
        private Rectangle _joystickBasePosition;

        public Texture2D JoystickTop
        {
            get { return _joystickTop; }
            set { _joystickTop = value; }
        }

        public Button FireButton
        {
            get { return _fireButton; }
            set { _fireButton = value; }
        }

        public Button MineButton
        {
            get { return _mineButton; }
            set { _mineButton = value; }
        }

        public VirtualGamepad(Texture2D joystickBase, Texture2D joystickTop, Button fireButton, Button mineButton)
        {
            this.JoystickBase = joystickBase;
            this.JoystickTop = joystickTop;
            this.FireButton = fireButton;
            this.MineButton = mineButton;

            _joystickBasePosition = JoystickBase.Bounds;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            float screenWidth = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            float screenHeight = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;

            FireButton.Draw(ref spriteBatch);
            MineButton.Draw(ref spriteBatch);

            _joystickBasePosition.Location = new Vector2((float) (screenWidth * 0.05), (float) (screenHeight * 0.62)).ToPoint();

            spriteBatch.Draw(JoystickBase, _joystickBasePosition.Location.ToVector2(), Color.White);
            
            var endJoyTopPosition = _joystickBasePosition.Location.ToVector2() -
                             JoystickTop.Bounds.Size.ToVector2() / new Vector2(2) +
                             (_joystickBasePosition.Size.ToVector2() / new Vector2(2)) * (_xyJoyMove + new Vector2(1));

            spriteBatch.Draw(JoystickTop, endJoyTopPosition, Color.White);
        }



        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            bool fire = false;
            bool plantMine = false;
            _xyJoyMove = new Vector2(0);

            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.Count == 0) return new TankControllerState(0,0);

            foreach (TouchLocation touch in touchState)
            {
                if (!_joystickBasePosition.Contains(touch.Position)) continue;

                var distanceFromCenter = touch.Position.ToPoint() - _joystickBasePosition.Location;
                distanceFromCenter -= _joystickBasePosition.Size / new Point(2);

                var halfJoySize = _joystickBasePosition.Size / new Point(2);

                _xyJoyMove = distanceFromCenter.ToVector2() / halfJoySize.ToVector2();
            }
            var pointerState = PointerState.GetState();

            if (FireButton.CheckIsMouseOver(ref pointerState))
                fire = true;
            if (MineButton.CheckIsMouseOver(ref pointerState))
                plantMine = true;

            return new TankControllerState(_xyJoyMove.X, -_xyJoyMove.Y, fire, false, plantMine);
        }

    }
}
