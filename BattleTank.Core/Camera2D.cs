using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleTank.Core
{
    class Camera2D
    {
        public double Scale { get; set; }
        public double Position { get; set; }

        public Point? MaxLeftTopCorner { get; set; } = null;

        public Point? MaxRightBottomCorner { get; set; } = null;

        public Matrix GetViewMatrix()
            => Camera2D.GetViewMatrix(Scale, Position, MaxLeftTopCorner, MaxRightBottomCorner);

        public static Matrix GetViewMatrix(double scale, double position, Point? maxLeftTopCorner = null,
            Point? maxRightBottomCorner = null)
        {
            return new Matrix();
        }
    }
}
