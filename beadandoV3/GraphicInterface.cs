using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadandoV3
{
    enum FieldType
    {
        EMPTY,
        SHIP,
        SHIP_HIT,
        MISSED

    }
    static class GraphicInterface
    {
        static public void AskShips(Player player, Game game)
        {

            for (int size = 4; size > 0; size--)
            {

                bool tryAgain = true;
                while (tryAgain)
                {
                    Console.WriteLine(PlayerName(player) + " hajói!");
                    Console.WriteLine("-----------------------------");
                    PrintField(player, game, false);
                    Console.WriteLine("A(z) {0} egység méretű hajójának koordinátái?", size);
                    AddShipResult result = game.addShip(new Ship(player, AskPoint(), size, (size == 1) ? Orientation.HORIZONTAL : AskOrientation()));

                    switch (result)
                    {
                        case AddShipResult.OK:
                            tryAgain = false;
                            break;
                        case AddShipResult.COLLISION:
                            Console.WriteLine("Egybe ütközik egy másik hajóval, próbáld újra! Enter a folytatáshoz...");
                            Console.ReadLine();
                            break;
                        case AddShipResult.OUT_OF_FIELD:
                            Console.WriteLine("Kilóg a hajó a tábláról! Enter a folytatáshoz...");
                            Console.ReadLine();
                            break;
                    }

                    Console.Clear();
                }
            }
        }
        static public void PrintField(Player player, Game game, bool target)
        {
            // Létrehozunk egy 2 dimenziós tömböt, ami a csatatér pontjait tárolja
            FieldType[,] field = new FieldType[10, 10];

            // Minden olyan hajó minden pontját beállítjuk, ami a player-é
            for (int shipIndex = 0; shipIndex < game.shipCount; shipIndex++)
            {
                if (game.ships[shipIndex].player == player)
                {
                    Point[] points = game.ships[shipIndex].GetPoints();
                    for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
                    {
                        field[points[pointIndex].x, points[pointIndex].y] = FieldType.SHIP;
                    }
                }
            }

            Player opponent = (player == Player.PLAYER_1) ? Player.PLAYER_2 : Player.PLAYER_1;
            for (int i = 0; i < game.moveCount; i++)
            {
                if (game.moves[i].player == opponent)
                {
                    int x = game.moves[i].point.x;
                    int y = game.moves[i].point.y;

                    field[x, y] = (field[x, y] == FieldType.SHIP) ? FieldType.SHIP_HIT : FieldType.MISSED;
                }
            }

            for (int y = field.GetLength(1) - 1; y >= 0; y--)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(y + " ");
                Console.ResetColor();
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    switch (field[x, y])
                    {
                        case FieldType.EMPTY:
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.Write("  ");
                            Console.ResetColor();
                            break;
                        case FieldType.SHIP:
                            if (target == true)
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.Write("  ");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.Write("  ");
                                Console.ResetColor();
                            }
                            break;
                        case FieldType.SHIP_HIT:
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("XX");
                            Console.ResetColor();
                            break;
                        case FieldType.MISSED:
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write("XX");
                            Console.ResetColor();
                            break;
                    }
                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  ");
            for (int i = 0; i < 10; i++)
            {
                Console.Write(i + " ");
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }


        static private string PlayerName(Player player)
        {
            return (player == Player.PLAYER_1) ? "Első Játékos" : "Második Játékos";
        }
        static private Point AskPoint()
        {
            Point result = new Point(0, 0);
            bool tryAgain = true;
            while (tryAgain)
            {
                Console.Write(" > x=");
                int x;
                bool isNumber = int.TryParse(Console.ReadLine(), out x);

                if (x >= 0 && x <= 9 && isNumber)
                {
                    tryAgain = false;
                }
                else
                {
                    tryAgain = true;
                    Console.WriteLine("Az X koordináta minimum 0, maximum 9 lehet!");
                }
                result.x = x;

            }
            tryAgain = true;
            while (tryAgain)
            {
                Console.Write(" > y=");
                int y;
                bool isNumber = int.TryParse(Console.ReadLine(), out y);

                if (y >= 0 && y <= 9 && isNumber)
                {
                    tryAgain = false;
                }
                else
                {
                    tryAgain = true;
                    Console.WriteLine("Az Y koordináta minimum 0, maximum 9 lehet!");
                }
                result.y = y;
            }

            return result;
        }

        static public bool AskForNextPlayerOrForfeit(Player player)
        {
            Console.WriteLine("Nyomjon egy entert a folytatáshoz vagy ird be, hogy 'forfeit' a feladashoz!");
            if (Console.ReadLine() == "forfeit")
            {
                return true;
            }
            Console.Clear();
            Console.WriteLine(PlayerName(player) + " Következik, a pálya megjelenítéséhez nyomjon egy entert!");
            Console.ReadLine();
            Console.Clear();
            return false;
        }
        static public bool AskForGameStart(Game game)
        {
            bool Starttype = true;
            Console.WriteLine("Üdv a Torpedó játékban!");
            Console.WriteLine("Szeretnéd folytatni az előző játékot, vagy új játékot kezdeni? Előző játék= I, Új játék= N");
            Console.WriteLine();
            bool isItok = true;
            while (isItok)
            {
                Console.Write("Válasz: ");
                string init = Console.ReadLine();
                if (init.ToLower()== "i")
                {
                    Starttype = true;
                    isItok = false;
                    Console.Clear();
                }
                else if (init.ToLower() == "n")
                {
                    Starttype = false;
                    isItok = false;
                    Console.Clear();
                    AskForUserLoad(game);
                }
                else
                {
                    Console.Clear();
                    Console.Write("Válaszolj újra: ");
                }
            }
            return Starttype;
        }
        static public void AskForUserLoad(Game game)
        {
            bool isItok = true;
            while (isItok)
            {
                Console.WriteLine("Szeretnél saját játékot betölteni?! (I/N)" );
                string answer = Console.ReadLine();
                if (answer.ToLower() == "i")
                {
                        Console.WriteLine("Fájl neve?(.txt nélkül)");
                        string fileNameByUser = Console.ReadLine();
                        game.load(fileNameByUser+"txt");
                        isItok = false;
                }
                else if (answer.ToLower() == "n")
                {
                    isItok = false;
                }
                else
                {
                    Console.WriteLine("Szóval?");
                }
            }
        }

        static private Orientation AskOrientation()
        {
            while (true)
            {
                Console.Write("Vízszintesen? I/N: ");
                string response = Console.ReadLine();
                if (response.ToLower() == "i")
                {
                    return Orientation.HORIZONTAL;
                }
                else if (response.ToLower() == "n")
                {
                    return Orientation.VERTICAL;
                }
            }
        }

        public static void AskMove(Player player, Game game)
        {
            Player opponent = (player == Player.PLAYER_1) ? Player.PLAYER_2 : Player.PLAYER_1;
            MoveResult mr = MoveResult.OUT_OF_FIELD;
            bool tryAgain = true;

            while (tryAgain)
            {
                Console.WriteLine(PlayerName(player));
                PrintField(opponent, game, true);
                PrintField(player, game, false);
                Console.WriteLine("Hova lojjek fonok?");
                mr = game.addMove(new Move(player, AskPoint()));

                switch (mr)
                {
                    case MoveResult.ALREADY_DONE:
                        Console.WriteLine("Ide már lőttél, próbáld újra!");
                        Console.ReadLine();
                        break;
                    case MoveResult.MISSED:
                    case MoveResult.HIT:
                        tryAgain = false;
                        break;
                }

                Console.Clear();
            }

            PrintField(opponent, game, true);
            Console.WriteLine();
            PrintField(player, game, false);
            Console.WriteLine();

            switch (mr)
            {
                case MoveResult.HIT:
                    Console.WriteLine("Ugyes vagy talat");
                    break;
                case MoveResult.MISSED:
                    Console.WriteLine("Elbasztad");
                    break;
            }
        }
        public static void PrintPlayerWon(Player player, Game game)
        {
            Console.Clear();
            PrintField(player, game, false);
            Player opponent = (player == Player.PLAYER_1) ? Player.PLAYER_2 : Player.PLAYER_1;
            PrintField(opponent, game, false);
            Console.WriteLine(PlayerName(player)+" nyerte a játékot! Gratu!");
        }
    }

}
