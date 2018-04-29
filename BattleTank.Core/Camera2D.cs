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

        private Vector2 VisibleScreen => new Vector2(_screenWidth / Scale, _screenHeight / Scale);

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
            Vector2 newPosition = Position;

            if (Center)
                newPosition -= new Vector2((_screenWidth / Scale) / 2, (_screenHeight / Scale) / 2);

            if (newPosition.X + VisibleScreen.X > MaxRightBottomCorner?.X)
                newPosition.X -= ((newPosition.X + VisibleScreen.X) - ((Point)MaxRightBottomCorner).X);

            if (newPosition.Y + VisibleScreen.Y > MaxRightBottomCorner?.Y)
                newPosition.Y -= ((newPosition.Y + VisibleScreen.Y) - ((Point)MaxRightBottomCorner).Y);

            if (newPosition.X < MaxLeftTopCorner?.X)
                newPosition.X += (((Point)MaxLeftTopCorner).X - newPosition.X);

            if (newPosition.Y < MaxLeftTopCorner?.Y)
                newPosition.Y += (((Point)MaxLeftTopCorner).Y - newPosition.Y);

            return Matrix.Identity * Matrix.CreateTranslation(-newPosition.X, -newPosition.Y, 0) * Matrix.CreateScale(Scale);
        }
    }
}
