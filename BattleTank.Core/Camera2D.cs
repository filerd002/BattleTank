using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    class Camera2D
    {
        private int _screenWidth;
        private int _screenHeight;
        public Camera2D(PresentationParameters parameters)
        {
            _screenHeight = parameters.BackBufferHeight;
            _screenWidth = parameters.BackBufferWidth;
        }
        public float Scale { get; set; }
        public bool Center { get; set; }
        public Vector2 Position { get; set; }

        public Point? MaxLeftTopCorner { get; set; } = null;

        public Point? MaxRightBottomCorner { get; set; } = null;

        public Matrix GetViewMatrix()
        {
            var newPosition = Position;
            if (Center)
            {
                newPosition  -= new Vector2((_screenWidth/Scale)/2, (_screenHeight/Scale)/2);   
            }
            return Matrix.Identity * Matrix.CreateTranslation(-newPosition.X, -newPosition.Y, 0) * Matrix.CreateScale(Scale);
        }
    }
}
