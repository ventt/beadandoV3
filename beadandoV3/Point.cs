using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadandoV3
{
    class Point
    {
        // Két koordináta
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        // Két pont vizsgálata
        public bool SameAs(Point b)
        {
            return x == b.x && y == b.y;
        }
    }
}
