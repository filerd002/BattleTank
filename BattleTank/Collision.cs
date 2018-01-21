using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleTank
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
