using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadandoV3
{
    class Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool SameAs(Point b)
        {
            return x == b.x && y == b.y;
        }
    }
}
