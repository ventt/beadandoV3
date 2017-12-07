using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadandoV3
{
    enum Orientation
    {
        VERTICAL, HORIZONTAL
    }

    class Ship
    {
        public Player player;
        public Point point;
        public int size;
        public Orientation orientation;

        public Ship(Player player, Point point, int size, Orientation orientation)
        {
            this.player = player;
            this.point = point;
            this.size = size;
            this.orientation = orientation;
        }
        public Point[] GetPoints()
        {
            Point[] points = new Point[size];

            if (orientation == Orientation.HORIZONTAL)
            {
                for (int i = 0; i < size; i++)
                {
                    points[i] = new Point(point.x + i, point.y);
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    points[i] = new Point(point.x, point.y + i);
                }
            }

            return points;
        }

        public override string ToString()
        {
            return (int)player + "," + point.x + "," + point.y + "," + size + "," + (int)orientation;
        }
    }
}
