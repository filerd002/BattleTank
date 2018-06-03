using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    public class Camera2D
    {
        public Vector2 ScreenSize { get; set; }
        private float _screenWidth => ScreenSize.X;
        private float _screenHeight => ScreenSize.Y;

        public Rectangle VisibleArea
        {
            get
            {
                var inverseViewMatrix = Matrix.Invert(GetViewMatrix());
                var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
                var tr = Vector2.Transform(new Vector2(ScreenSize.X, 0), inverseViewMatrix);
                var bl = Vector2.Transform(new Vector2(0, ScreenSize.Y), inverseViewMatrix);
                var br = Vector2.Transform(ScreenSize, inverseViewMatrix);
                var min = new Vector2(
                    MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                    MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
                var max = new Vector2(
                    MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                    MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
                return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }

        public Camera2D(PresentationParameters parameters)
        {
            ScreenSize = new Vector2(parameters.BackBufferWidth, parameters.BackBufferHeight);
        }
        public float Scale { get; set; }
        public bool Center { get; set; }
        public Vector2 Position { get; set; }

        public Point? MaxLeftTopCorner { get; set; } = null;

        public Point? MaxRightBottomCorner { get; set; } = null;

        public Matrix GetViewMatrix()
        {
            Vector2 newPosition = Position;
            Vector2 visibleScreen = new Vector2(_screenWidth / Scale, _screenHeight / Scale);

            if (Center)
                newPosition -= new Vector2((_screenWidth / Scale) / 2, (_screenHeight / Scale) / 2);

            if (newPosition.X + visibleScreen.X > MaxRightBottomCorner?.X)
                newPosition.X -= ((newPosition.X + visibleScreen.X) - ((Point)MaxRightBottomCorner).X);

            if (newPosition.Y + visibleScreen.Y > MaxRightBottomCorner?.Y)
                newPosition.Y -= ((newPosition.Y + visibleScreen.Y) - ((Point)MaxRightBottomCorner).Y);

            if (newPosition.X < MaxLeftTopCorner?.X)
                newPosition.X += (((Point)MaxLeftTopCorner).X - newPosition.X);

            if (newPosition.Y < MaxLeftTopCorner?.Y)
                newPosition.Y += (((Point)MaxLeftTopCorner).Y - newPosition.Y);

            return Matrix.Identity * Matrix.CreateTranslation(-newPosition.X, -newPosition.Y, 0) * Matrix.CreateScale(Scale);
        }
    }
}
