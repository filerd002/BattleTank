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
        private Vector2 _xyJoyMove;
        
        public Texture2D JoystickBase { get; set; }

        private Rectangle _joystickBasePosition;

        public Texture2D JoystickTop { get; set; }

        public Button FireButton { get; set; }

        public Button MineButton { get; set; }

        public DirectionsOfMovement DOM { get; set; }

        public enum DirectionsOfMovement
        {
            BASIC,
            ADVANCED       
        }

        public VirtualGamepad(Texture2D joystickBase, Texture2D joystickTop, Button fireButton, Button mineButton, DirectionsOfMovement directionsOfMovement )
        {
            this.JoystickBase = joystickBase;
            this.JoystickTop = joystickTop;
            this.FireButton = fireButton;
            this.MineButton = mineButton;
            this.DOM = directionsOfMovement;

            _joystickBasePosition = JoystickBase.Bounds;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            float screenWidth = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            float screenHeight = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;

            FireButton.Draw(ref spriteBatch);
            MineButton.Draw(ref spriteBatch);

           
            _joystickBasePosition.Location = new Vector2((float) (screenWidth * 0.05), (float) (screenHeight * 0.62)).ToPoint();
            _joystickBasePosition.Size =  new Vector2((float)(screenWidth * 0.175), (float)(screenWidth * 0.175)).ToPoint();
            spriteBatch.Draw(JoystickBase, new Rectangle((int)_joystickBasePosition.Location.ToVector2().X, (int)_joystickBasePosition.Location.ToVector2().Y, (int)_joystickBasePosition.Size.ToVector2().X, (int)_joystickBasePosition.Size.ToVector2().Y), Color.White);

            Vector2 endJoyTopSize = new Vector2(_joystickBasePosition.Size.X / 2, _joystickBasePosition.Size.Y / 2);
            Vector2 endJoyTopPosition = _joystickBasePosition.Location.ToVector2() -  endJoyTopSize / new Vector2(2) +  (_joystickBasePosition.Size.ToVector2() / new Vector2(2)) * (_xyJoyMove + new Vector2(1));

            spriteBatch.Draw(JoystickTop, new Rectangle((int)endJoyTopPosition.X, (int)endJoyTopPosition.Y, (int)endJoyTopSize.X, (int)endJoyTopSize.Y), Color.White);
        }

        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            bool fire = false;
            bool plantMine = false;
            bool speedUp = false;
            _xyJoyMove = new Vector2(0);

            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.Count == 0) return new TankControllerState(0,0);

            foreach (TouchLocation touch in touchState)
            {
                // Rozszerzona podstawa służy do tego, aby umożliwić efekt przyśpieszenia przy odpowiednio
                // mocno wychylonej gałce.
                Rectangle extendendBaseSize = _joystickBasePosition;
                extendendBaseSize.Size += new Point(100);
                extendendBaseSize.Location -= new Point(50);

                if (!extendendBaseSize.Contains(touch.Position)) continue;

                Point distanceFromCenter = touch.Position.ToPoint() - _joystickBasePosition.Location;
                distanceFromCenter -= _joystickBasePosition.Size / new Point(2);

                Point halfJoySize = _joystickBasePosition.Size / new Point(2);

                _xyJoyMove = distanceFromCenter.ToVector2() / halfJoySize.ToVector2();
                if (_xyJoyMove.X > 1 || _xyJoyMove.Y > 1 || _xyJoyMove.X < -1 || _xyJoyMove.Y < -1)
                    speedUp = true;
            }
            PointerState pointerState = PointerState.GetState();

            if (FireButton.CheckIsMouseOver(ref pointerState))
                fire = true;
            if (MineButton.CheckIsMouseOver(ref pointerState))
                plantMine = true;

            if (DOM.Equals(DirectionsOfMovement.BASIC))
            {
                _xyJoyMove.X = Convert.ToInt32(_xyJoyMove.X);
                _xyJoyMove.Y = Convert.ToInt32(_xyJoyMove.Y);
            }

            return new TankControllerState(_xyJoyMove.X, -_xyJoyMove.Y, fire, speedUp, plantMine, true);
        }

        public bool IsConnectedTankController()
        {
            if (TouchPanel.GetState().IsConnected)
                return true;
            else
                return false;
        }
    }
}
