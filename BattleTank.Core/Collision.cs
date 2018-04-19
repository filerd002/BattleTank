namespace BattleTank.Core
{
    public class Collision
    {
        public enum Side { LEFT, RIGHT, TOP, BOTTOM };
        public Side side;
        public float depth;
        public Collision() { }
        public Collision(Side _side, float _depth)
        {
            side = _side;
            depth = _depth;
        }
    }
}
