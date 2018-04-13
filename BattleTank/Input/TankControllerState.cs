using System;
using System.Diagnostics;

namespace BattleTank.Input
{
    public struct TankControllerState
    {
        
        public float MoveY { get; }
        public float MoveX { get; }
        public bool Fire { get; }
        public bool SpeedBoost { get; }
        public bool PlantMine { get; }
        public TankControllerState(float moveX, float moveY, bool fire = false, bool speedBoost = false, bool plantMine = false, bool safely = false) : this()
        {
            if (safely)
            {
                if (Math.Abs(moveX) > 1) moveX = 1 * Math.Sign(moveX);
                if (Math.Abs(moveY) > 1) moveY = 1 * Math.Sign(moveY);
            }
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
        /// <summary>
        /// Powoduje, że wartości X, Y są zmieniane o zadany procent.
        /// Gwarantuje ona, że wartości X i Y nie przekroczą wartości maksymalnych MoveX i MoveY, czyli będą w zakresie -1:1.
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public TankControllerState SafelySpeedUp(float percent)
            => new TankControllerState(MoveX * percent, MoveY * percent, Fire, SpeedBoost, PlantMine, true);

#if DEBUG
        /// <inheritdoc />
        public override string ToString()
            => MoveX + " x " + MoveY + " F:" + (Fire ? "Y" : "N") + " S:" + (SpeedBoost ? "Y" : "N") + " P:" +
               (PlantMine ? "Y" : "N");
#endif
    }
}
