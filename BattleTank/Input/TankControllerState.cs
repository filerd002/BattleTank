namespace BattleTank.Input
{
    public struct TankControllerState
    {
        
        public int MoveY { get; }
        public int MoveX { get; }
        public bool Fire { get; }
        public bool SpeedBoost { get; }
        public bool PlantMine { get; }
        public TankControllerState(int moveY, int moveX, bool fire, bool speedBoost, bool plantMine) : this()
        {
            MoveY = moveY;
            MoveX = moveX;
            Fire = fire;
            SpeedBoost = speedBoost;
            PlantMine = plantMine;
        }
    }
}
