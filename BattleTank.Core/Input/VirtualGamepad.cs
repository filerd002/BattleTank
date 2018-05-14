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
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            float screenWidth = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            float screenHeight = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;

            FireButton.Draw(ref spriteBatch);
            MineButton.Draw(ref spriteBatch);

            var basePosition = new Vector2((float) (screenWidth * 0.05), (float) (screenHeight * 0.62));
            spriteBatch.Draw(JoystickBase, basePosition, Color.White);
            var joyTopPosition = basePosition + new Vector2(JoystickBase.Width/2, JoystickBase.Height/2) - new Vector2(JoystickTop.Width/2, JoystickTop.Height/2);
            spriteBatch.Draw(JoystickTop, joyTopPosition, Color.White);
        }

        public float Size = 300;
        public Vector2 StartPosition = Vector2.Zero;
        public int Id = -1;

        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            TankControllerState retVal = new TankControllerState(0, 0);
            TouchCollection touchState = TouchPanel.GetState();
            if (touchState.Count == 0)
            {
                Id = -1;
                StartPosition = Vector2.Zero;
                return retVal;
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

                float xMove = StartPosition.X - touch.Position.X;
                xMove = (Math.Abs(xMove) > Size ? 1 * Math.Sign(xMove) : xMove / Size);

                float yMove = StartPosition.Y - touch.Position.Y;
                yMove = (Math.Abs(yMove) > Size ? 1 * Math.Sign(yMove) : yMove / Size);

                retVal = new TankControllerState(-xMove, yMove);
            }
            return retVal;
        }

    }
}
