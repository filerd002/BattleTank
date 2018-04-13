using System;

namespace BattleTank.Input
{
    public struct TankControllerState
    {
        
        public float MoveY { get; }
        public float MoveX { get; }
        public bool Fire { get; }
        public bool SpeedBoost { get; }
        public bool PlantMine { get; }
        public TankControllerState(float moveX, float moveY, bool fire = false, bool speedBoost = false, bool plantMine = false) : this()
        {
           
            if (Math.Abs(moveY) > 1)
               throw new ArgumentOutOfRangeException(nameof(moveY), moveY, "Wartość musi byc w zakresie -1 <= Y <= 1");
            if (Math.Abs(moveX) > 1)
               throw new ArgumentOutOfRangeException(nameof(moveX), moveX, "Wartość musi byc w zakresie -1 <= X <= 1");

            MoveY = moveY;
            MoveX = moveX;
            Fire = fire;
            SpeedBoost = speedBoost;
            PlantMine = plantMine;
        }

        public TankControllerState Rotate(double angle)
        {
            double X = (double)(MoveX * Math.Cos(angle) - MoveY * Math.Sin(angle));
            double Y = (double)(MoveX * Math.Sin(angle) + MoveY * Math.Cos(angle));

            X = Math.Abs(X) > 1 ? Math.Truncate(X) : X;
            Y = Math.Abs(Y) > 1 ? Math.Truncate(Y) : Y;

            return new TankControllerState(
                (float)X,
                (float)Y,
                Fire, SpeedBoost, PlantMine);
        }
    }
}
