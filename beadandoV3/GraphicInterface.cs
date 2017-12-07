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

    enum AskNextResult
    {
        NEXT_PLAYER,
        FORFEIT,
        SAVE
    }
    class GraphicInterface
    {
        private Game game;

        public GraphicInterface(Game game)
        {
            this.game = game;
        }

        public void AskShips(Player player)
        {

            for (int size = 4; size > 0; size--)
            {

                bool tryAgain = true;
                while (tryAgain)
                {
                    Console.WriteLine(PlayerName(player) + " hajói!");
                    Console.WriteLine("-----------------------------");
                    PrintField(player, false);
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
        public void PrintField(Player player, bool target)
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


        private string PlayerName(Player player)
        {
            return (player == Player.PLAYER_1) ? "Első Játékos" : "Második Játékos";
        }
        private Point AskPoint()
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

        public AskNextResult AskNext(Player player)
        {
            Console.WriteLine("Nyomjon egy entert a folytatáshoz vagy ird be, hogy 'forfeit' a feladashoz, vagy 'save' a mentéshez!");
            string command = Console.ReadLine();
            if (command == "forfeit")
            {
                return AskNextResult.FORFEIT;
            }
            else if (command == "save")
            {
                Console.WriteLine("Adja meg a mentés nevét!(.txt nélkül)");
                Console.Write(" > ");
                string fileNameByUser = Console.ReadLine();
                game.fileName = "../.../" + fileNameByUser + ".txt";
                game.Save();
                return AskNextResult.SAVE;
            }
            Console.Clear();
            Console.WriteLine(PlayerName(player) + " Következik, a pálya megjelenítéséhez nyomjon egy entert!");
            Console.ReadLine();
            Console.Clear();
            return AskNextResult.NEXT_PLAYER;
        }
        public bool AskForGameStart()
        {
            Console.WriteLine("Üdv a Torpedó játékban!");
            Console.WriteLine("Szeretnéd folytatni az előző játékot, vagy új játékot kezdeni? Előző játék= I, Új játék= N");
            Console.WriteLine();
            while (true)
            {
                Console.Write("Válasz: ");
                string init = Console.ReadLine();
                if (init.ToLower() == "i")
                {
                    game.load(game.fileName);
                    return false;
                }
                else if (init.ToLower() == "n")
                {
                    return AskForUserLoad();
                }
                else
                {
                    Console.Clear();
                    Console.Write("Válaszolj újra: ");
                }
            }
        }
        public bool AskForUserLoad()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Szeretnél saját játékot betölteni?! (I/N)");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "i")
                {
                    Console.WriteLine("Fájl neve?(.txt nélkül)");
                    string fileNameByUser = Console.ReadLine();
                    game.load("../.../" + fileNameByUser + ".txt");
                    return false;
                }
                else if (answer.ToLower() == "n")
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Szóval?");
                }
            }
        }

        private Orientation AskOrientation()
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

        public void AskMove(Player player)
        {
            Player opponent = (player == Player.PLAYER_1) ? Player.PLAYER_2 : Player.PLAYER_1;
            MoveResult mr = MoveResult.OUT_OF_FIELD;
            bool tryAgain = true;

            while (tryAgain)
            {
                Console.WriteLine(PlayerName(player));
                PrintField(opponent, true);
                PrintField(player, false);
                Console.WriteLine("Hova löjjek főnök?");
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

            PrintField(opponent, true);
            Console.WriteLine();
            PrintField(player, false);
            Console.WriteLine();

            switch (mr)
            {
                case MoveResult.HIT:
                    Console.WriteLine("Talált!");
                    break;
                case MoveResult.MISSED:
                    Console.WriteLine("Nem talált");
                    break;
            }
        }
        public void PrintPlayerWon(Player player)
        {
            Console.Clear();
            PrintField(player, false);
            Player opponent = (player == Player.PLAYER_1) ? Player.PLAYER_2 : Player.PLAYER_1;
            PrintField(opponent, false);
            Console.WriteLine(PlayerName(player) + " nyerte a játékot!");
        }
    }

}
