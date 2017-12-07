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

    enum MoveResult   //Lépés típusok
    {
        HIT,
        MISSED,
        OUT_OF_FIELD,
        ALREADY_DONE
    }
    enum AddShipResult   //Hajó lerakási típusok
    {
        OK,
        COLLISION,
        OUT_OF_FIELD
    }
    class Game
    {
        public Ship[] ships = new Ship[8];  //Maximum hajók száma
        public int shipCount = 0;  // Eddigi hajók száma

        public Move[] moves = new Move[200];  // Maximum lépések száma
        public int moveCount = 0;  // Eddigi lépések száma

        public string fileName = "../../game.txt";

        public Player getCurrentPlayer()
        {
            // Megmondja, hogy melyik játékos következi oszthatóság által
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
            // Megmondja az egyik playerről, hogy melyik az ellenfele
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
                // Megnézzük hogy a hajók kilógnak-e (horizontálisan)
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
                // Megnézzük hogy a hajók kilógnak-e (vertikálisan)
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
            // Megnézzük hogy nyert-e egy játékos
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
            // Meglévő fájból való betöltés Input: fájl elérhetőség
        {
            StreamReader sr = new StreamReader(file);

            for (int i = 0; i < 8; i++)
            {
                // Array.ConvertAll-al lehet minden egyes splitet, int-re castolni (1. Játékos, 2. x 3. y 4. Orientáció)
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

        public void Save()
        {
            // Univerzális mentés Fájl nevet kell megadni, az Exe-t tartalmazó mappába ment
            StreamWriter sw = new StreamWriter(fileName);

            foreach (Ship ship in ships)
            {
                sw.WriteLineAsync(ship.ToString());  
            }

            for (int i = 0; i < moveCount; i++)
            {
                sw.WriteLineAsync(moves[i].ToString());
            }

            sw.Flush();  //Egybe kimenti a fájlba
            sw.Close();
        }


        private bool isPointInField(Point point)
        {
            // Ellenörzi hogy a pont a pályán belül van-e
            return point.x >= 0 && point.x <= 9 && point.y >= 0 && point.y <= 9;

        }

        private Ship searchShipInPoint(Point point, Player player)
        {
            // Végig fut az eddig lerakott hajókon (shipCount)
            // Megnézi, hogy melyik hajoban van ez a pont, ha nincs ilyen hajo, akkor null-t add vissza
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
