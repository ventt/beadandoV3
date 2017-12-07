using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace beadandoV3
{
    enum Player
    {
        PLAYER_1,
        PLAYER_2
    }

    enum MoveResult
    {
        HIT,
        MISSED,
        OUT_OF_FIELD,
        ALREADY_DONE
    }
    enum AddShipResult
    {
        OK,
        COLLISION,
        OUT_OF_FIELD
    }
    class Game
    {
        public Ship[] ships = new Ship[8];
        public int shipCount = 0;

        public Move[] moves = new Move[200];
        public int moveCount = 0;
        public Player getCurrentPlayer()
        {
            if (moveCount % 2 == 0)
            {
                return Player.PLAYER_1;
            }
            else
            {
                return Player.PLAYER_2;
            }
        }
        public Player getCurrentOpponent()
        {
            if (moveCount % 2 == 0)
            {
                return Player.PLAYER_2;
            }
            else
            {
                return Player.PLAYER_1;
            }
        }
        public AddShipResult addShip(Ship ship)
        {
            if (ship.orientation == Orientation.HORIZONTAL)
            {
                if (!isPointInField(ship.point) || !isPointInField(new Point(ship.point.x + ship.size - 1, ship.point.y)))
                {
                    return AddShipResult.OUT_OF_FIELD;
                }
                // Megnézzük hogy a hajók összeérnek-e (horizontálisan)
                for (int i = 0; i < ship.size; i++)
                {
                    if (searchShipInPoint(new Point(ship.point.x + i, ship.point.y), ship.player) != null)
                    {
                        return AddShipResult.COLLISION;
                    }
                }
            }
            else
            {
                if (!isPointInField(ship.point) || !isPointInField(new Point(ship.point.x, ship.point.y + ship.size - 1)))
                {
                    return AddShipResult.OUT_OF_FIELD;
                }
                // Megnézzük hogy a hajók összeérnek-e (vertikálisan)
                for (int i = 0; i < ship.size; i++)
                {
                    if (searchShipInPoint(new Point(ship.point.x, ship.point.y + i), ship.player) != null)
                    {
                        return AddShipResult.COLLISION;
                    }
                }

            }

            ships[shipCount++] = ship;
            return AddShipResult.OK;
        }



        public MoveResult addMove(Move move)
        {

            // Megnezzuk, hogy a palyan van-e
            if (move.point.x < 0 || move.point.x > 9 || move.point.y < 0 || move.point.y > 9)
            {
                return MoveResult.OUT_OF_FIELD;
            }

            // Megnezzuk, hogy lepett-e mar ilyet
            for (int i = 0; i < moveCount; i++)
            {
                if (moves[i].player == move.player && moves[i].point.SameAs(move.point))
                {
                    return MoveResult.ALREADY_DONE;
                }
            }



            // Megnezzuk, hogy eltalalt-e egy hajot
            if (searchShipInPoint(move.point, getCurrentOpponent()) != null)
            {
                moves[moveCount++] = move;
                return MoveResult.HIT;
            }
            else
            {
                // Eltaroljuk a lepest ha talalt vagy nem talalt
                moves[moveCount++] = move;
                return MoveResult.MISSED;
            }
        }

        public bool hasPlayerWon(Player player)
        {
            Player opponent = (player == Player.PLAYER_1) ? Player.PLAYER_2 : Player.PLAYER_1;
            int hitCount = 0;
            for (int i = 0; i < moveCount; i++)
            {
                if (moves[i].player == player && searchShipInPoint(moves[i].point, opponent) != null)
                {
                    if (++hitCount == 10)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void load(string file)
        {
            StreamReader sr = new StreamReader(file);

            for (int i = 0; i < 8; i++)
            {
                int[] nums = Array.ConvertAll(sr.ReadLine().Split(','), int.Parse);
                ships[shipCount++] = new Ship((Player)nums[0], new Point(nums[1], nums[2]), nums[3], (Orientation)nums[4]);
            }

            while (!sr.EndOfStream)
            {
                int[] nums = Array.ConvertAll(sr.ReadLine().Split(','), int.Parse);
                moves[moveCount++] = new Move((Player)nums[0], new Point(nums[1], nums[2]));
            }


            sr.Close();
        }

        public void Save(string file,bool saveByUser)
        {
            if (saveByUser)
            {
                Console.WriteLine("Adja meg a mentés nevét!(.txt nélkül)");
                string fileNameByUser = Console.ReadLine();
                StreamWriter sw = new StreamWriter("../.../"+fileNameByUser +"txt");
                foreach (Ship ship in ships)
                {
                    sw.WriteLineAsync(ship.ToString());
                }

                for (int i = 0; i < moveCount; i++)
                {
                    sw.WriteLineAsync(moves[i].ToString());
                }

                sw.Flush();
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(file);

                foreach (Ship ship in ships)
                {
                    sw.WriteLineAsync(ship.ToString());
                }

                for (int i = 0; i < moveCount; i++)
                {
                    sw.WriteLineAsync(moves[i].ToString());
                }

                sw.Flush();
                sw.Close();
            }
            
        }


        private bool isPointInField(Point point)
        {
            return point.x >= 0 && point.x <= 9 && point.y >= 0 && point.y <= 9;

        }

        private Ship searchShipInPoint(Point point, Player player)
        {
            // Megnezzuk, hogy melyik hajoban van ez a pont, ha nincs ilyen hajo, akkor null-t adunk vissza
            for (int i = 0; i < shipCount; i++)
            {
                if (player == ships[i].player)
                {
                    if (ships[i].orientation == Orientation.VERTICAL)
                    {
                        if (point.x == ships[i].point.x && ships[i].point.y <= point.y && ships[i].point.y + ships[i].size - 1 >= point.y)
                        {
                            return ships[i];
                        }
                    }
                    else
                    {
                        if (point.y == ships[i].point.y && ships[i].point.x <= point.x && ships[i].point.x + ships[i].size - 1 >= point.x)
                        {
                            return ships[i];
                        }
                    }
                }
            }
            return null;
        }

    }
}
