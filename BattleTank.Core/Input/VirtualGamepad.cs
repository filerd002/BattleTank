using System;
using BattleTank.Core.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BattleTank.Core.Input
{
    class VirtualGamepad : ITankActionProvider
    {
        private Button _mineButton;
        private Button _fireButton;
        private Texture2D _joystickTop;
        private Texture2D _joystickBase;
        
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
            
            var joyTopPosition = _joystickBasePosition.Location.ToVector2() + new Vector2(JoystickBase.Width/2, JoystickBase.Height/2) - new Vector2(JoystickTop.Width/2, JoystickTop.Height/2);
            spriteBatch.Draw(JoystickTop, joyTopPosition, Color.White);
        }

        public float Size = 300;
        public Vector2 StartPosition = Vector2.Zero;
        public int Id = -1;

        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            float xMove = 0;
            float yMove = 0;
            bool fire = false;
            bool plantMine = false;

            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.Count == 0)
            {
                Id = -1;
                StartPosition = Vector2.Zero;
                return new TankControllerState(0,0);
            }

            foreach (TouchLocation touch in touchState)
            {
                if (touch.State != TouchLocationState.Moved) continue;
                if (touch.Id != Id && Id != -1) continue;

                Id = touch.Id;
                if (StartPosition == Vector2.Zero)
                {
                    if (touch.TryGetPreviousLocation(out TouchLocation previousLocation))
                    {
                        StartPosition = previousLocation.Position;
                    }
                }

                xMove = StartPosition.X - touch.Position.X;
                xMove = -((Math.Abs(xMove) > Size ? 1 * Math.Sign(xMove) : xMove / Size));

                yMove = StartPosition.Y - touch.Position.Y;
                yMove = (Math.Abs(yMove) > Size ? 1 * Math.Sign(yMove) : yMove / Size);
            }
            var pointerState = PointerState.GetState();

            if (FireButton.CheckIsMouseOver(ref pointerState))
                fire = true;
            if (MineButton.CheckIsMouseOver(ref pointerState))
                plantMine = true;

            return new TankControllerState(xMove, yMove, fire, false, plantMine);
        }

    }
}
