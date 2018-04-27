using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleTank.Core
{
    class Camera2D
    {
        public Camera2D(PresentationParameters parameters)
        {
            
        }
        public double Scale { get; set; }
        public bool Center { get; set; }
        public Vector2 Position { get; set; }

        public Point? MaxLeftTopCorner { get; set; } = null;

        public Point? MaxRightBottomCorner { get; set; } = null;

        public Matrix GetViewMatrix()
            => Camera2D.GetViewMatrix(Scale, Position, MaxLeftTopCorner, MaxRightBottomCorner);

        public static Matrix GetViewMatrix(double scale, Vector2 position, Point? maxLeftTopCorner = null,
            Point? maxRightBottomCorner = null)
        {
            return Matrix.Identity;
        }
    }
}
