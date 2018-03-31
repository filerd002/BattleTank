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
        public TankControllerState(float moveY, float moveX, bool fire, bool speedBoost, bool plantMine) : this()
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
    }
}
