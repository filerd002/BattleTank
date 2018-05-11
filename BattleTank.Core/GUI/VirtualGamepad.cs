using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core.GUI
{
    class VirtualGamepad
    {
        private Texture2D _mineButton;
        private Texture2D _fireButton;
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

        public Texture2D FireButton
        {
            get { return _fireButton; }
            set { _fireButton = value; }
        }

        public Texture2D MineButton
        {
            get { return _mineButton; }
            set { _mineButton = value; }
        }

        public VirtualGamepad(Texture2D joystickBase, Texture2D joystickTop, Texture2D fireButton, Texture2D mineButton)
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

            spriteBatch.Draw(FireButton, new Vector2((float) (screenWidth*0.88), (float) (screenHeight*0.58)), null, Color.White);
            spriteBatch.Draw(MineButton, new Vector2((float) (screenWidth*0.78), (float) (screenHeight*0.78)), null, Color.White);

            var basePosition = new Vector2((float) (screenWidth * 0.05), (float) (screenHeight * 0.62));
            spriteBatch.Draw(JoystickBase, basePosition, Color.White);
            var joyTopPosition = basePosition + new Vector2(JoystickBase.Width/2, JoystickBase.Height/2) - new Vector2(JoystickTop.Width/2, JoystickTop.Height/2);
            spriteBatch.Draw(JoystickTop, joyTopPosition, Color.White);
        }

    }
}
