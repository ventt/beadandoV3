using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadandoV3
{
    class Move
    {
        public Player player;
        public Point point;

        public Move(Player player, Point point)
        {
            this.player = player;
            this.point = point;
        }

        public override string ToString()
        {
            return (int)player + "," + point.x + "," + point.y;
        }
    }
}
