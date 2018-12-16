namespace BattleTank.Core
{
    public class Collision
    {
        public enum Side { LEFT, RIGHT, TOP, BOTTOM };

        public float Depth { get; set; }
        public Side SideCollision { get; set; }

        public Collision() { }
        public Collision(Side _side, float _depth)
        {
            SideCollision = _side;
            Depth = _depth;
        }
    }
}
