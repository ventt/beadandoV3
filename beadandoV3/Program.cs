using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadandoV3
{

    class Program
    {

        static void Main(string[] args)
        {

            TestAddMove();
            TestAddShip();
            TestHasPlayerWon();
            TestShip();

            Game game = new Game();
            //GraphicInterface.AskShips(Player.PLAYER_1, game);
            //GraphicInterface.AskShips(Player.PLAYER_2, game);

            game.addShip(new Ship(Player.PLAYER_1, new Point(2, 2), 2, Orientation.HORIZONTAL));
            game.addShip(new Ship(Player.PLAYER_1, new Point(3, 4), 3, Orientation.HORIZONTAL));
            game.addShip(new Ship(Player.PLAYER_1, new Point(7, 2), 4, Orientation.VERTICAL));
            game.addShip(new Ship(Player.PLAYER_1, new Point(3, 6), 1, Orientation.HORIZONTAL));

            game.addShip(new Ship(Player.PLAYER_2, new Point(0, 0), 2, Orientation.VERTICAL));
            game.addShip(new Ship(Player.PLAYER_2, new Point(5, 4), 3, Orientation.HORIZONTAL));
            game.addShip(new Ship(Player.PLAYER_2, new Point(3, 2), 4, Orientation.VERTICAL));
            game.addShip(new Ship(Player.PLAYER_2, new Point(7, 6), 1, Orientation.HORIZONTAL));

            game.addMove(new Move(Player.PLAYER_1, new Point(0, 0)));
            game.addMove(new Move(Player.PLAYER_2, new Point(0, 0)));
            game.addMove(new Move(Player.PLAYER_1, new Point(0, 1)));
            game.addMove(new Move(Player.PLAYER_2, new Point(0, 1)));

            game.addMove(new Move(Player.PLAYER_1, new Point(5, 4)));
            game.addMove(new Move(Player.PLAYER_2, new Point(0, 2)));
            game.addMove(new Move(Player.PLAYER_1, new Point(6, 4)));
            game.addMove(new Move(Player.PLAYER_2, new Point(0, 3)));
            game.addMove(new Move(Player.PLAYER_1, new Point(7, 4)));
            game.addMove(new Move(Player.PLAYER_2, new Point(0, 4)));


            game.addMove(new Move(Player.PLAYER_1, new Point(3, 2)));
            game.addMove(new Move(Player.PLAYER_2, new Point(1, 0)));
            game.addMove(new Move(Player.PLAYER_1, new Point(3, 3)));
            game.addMove(new Move(Player.PLAYER_2, new Point(2, 0)));
            game.addMove(new Move(Player.PLAYER_1, new Point(3, 4)));
            game.addMove(new Move(Player.PLAYER_2, new Point(3, 0)));
            game.addMove(new Move(Player.PLAYER_1, new Point(3, 5)));
            game.addMove(new Move(Player.PLAYER_2, new Point(4, 0)));

            while (true)
            {
                game.save();

                Player currentplayer = game.getCurrentPlayer();
                Player currentOpponent = game.getCurrentOpponent();

                GraphicInterface.AskMove(currentplayer, game);
                if (game.hasPlayerWon(currentplayer))
                {
                    GraphicInterface.PrintPlayerWon(currentplayer, game);
                    break;
                }
                bool forfeit = GraphicInterface.AskForNextPlayerOrForfeit(currentOpponent);

                if (forfeit)
                {
                    GraphicInterface.PrintPlayerWon(currentOpponent, game);
                    break;
                }
            }

            Console.ReadLine();
        }
        static void TestAddMove()
        {
            // Add move test
            Game game = new Game();

            game.ships[0] = new Ship(Player.PLAYER_2, new Point(2, 7), 2, Orientation.HORIZONTAL);
            game.ships[1] = new Ship(Player.PLAYER_1, new Point(7, 4), 3, Orientation.VERTICAL);
            game.ships[2] = new Ship(Player.PLAYER_1, new Point(0, 0), 3, Orientation.HORIZONTAL);
            game.ships[3] = new Ship(Player.PLAYER_2, new Point(0, 0), 4, Orientation.HORIZONTAL);
            game.shipCount = 4;

            if (game.addMove(new Move(Player.PLAYER_1, new Point(3, 2))) != MoveResult.MISSED)
            {
                throw new Exception("Add move MISSED test FAILED ");
            }

            if (game.addMove(new Move(Player.PLAYER_2, new Point(2, 0))) != MoveResult.HIT)
            {
                throw new Exception("Add move HIT test FAILED");
            }

            if (game.addMove(new Move(Player.PLAYER_1, new Point(3, 2))) != MoveResult.ALREADY_DONE)
            {
                throw new Exception("Add move ALREADY_DONE test FAILED");
            }

            if (game.addMove(new Move(Player.PLAYER_1, new Point(10, 2))) != MoveResult.OUT_OF_FIELD)
            {
                throw new Exception("Add move OUT_OF_FIELD test FAILED");
            }

            if (game.addMove(new Move(Player.PLAYER_1, new Point(1, 0))) != MoveResult.HIT)
            {
                throw new Exception("Add move HIT 2 test FAILED");
            }

            if (game.addMove(new Move(Player.PLAYER_2, new Point(7, 6))) != MoveResult.HIT)
            {
                throw new Exception("Add move HIT 3 test FAILED");
            }
            if (game.addMove(new Move(Player.PLAYER_1, new Point(7, 6))) != MoveResult.MISSED)
            {
                throw new Exception("Add move MISSED 2 test FAILED");
            }
        }
        static void TestAddShip()
        {
            Game game = new Game();

            if (game.addShip(new Ship(Player.PLAYER_1, new Point(2, 3), 3, Orientation.HORIZONTAL)) != AddShipResult.OK)
            {
                throw new Exception("Add ship OK 1 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(2, 3), 3, Orientation.HORIZONTAL)) != AddShipResult.COLLISION)
            {
                throw new Exception("Add ship COLLISION 1 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(3, 2), 3, Orientation.VERTICAL)) != AddShipResult.COLLISION)
            {
                throw new Exception("Add ship COLLISION 2 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(2, 0), 3, Orientation.HORIZONTAL)) != AddShipResult.OK)
            {
                throw new Exception("Add ship OK 2 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(7, 0), 4, Orientation.HORIZONTAL)) != AddShipResult.OUT_OF_FIELD)
            {
                throw new Exception("Add ship OUT_OF_FIELD 1 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(7, 7), 4, Orientation.VERTICAL)) != AddShipResult.OUT_OF_FIELD)
            {
                throw new Exception("Add ship OUT_OF_FIELD 2 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(7, 8), 3, Orientation.HORIZONTAL)) != AddShipResult.OK)
            {
                throw new Exception("Add ship OK 3 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(8, 7), 4, Orientation.VERTICAL)) != AddShipResult.OUT_OF_FIELD)
            {
                throw new Exception("Add ship OUT_OF_FIELD 3 test FAILED");
            }
            if (game.addShip(new Ship(Player.PLAYER_1, new Point(9, 9), 1, Orientation.HORIZONTAL)) != AddShipResult.OK)
            {
                throw new Exception("Add ship OK 1 test FAILED");
            }

        }
        static void TestHasPlayerWon()
        {
            Game game = new Game();
            game.addShip(new Ship(Player.PLAYER_1, new Point(2, 2), 2, Orientation.HORIZONTAL));
            game.addShip(new Ship(Player.PLAYER_1, new Point(3, 4), 3, Orientation.HORIZONTAL));
            game.addShip(new Ship(Player.PLAYER_1, new Point(7, 2), 4, Orientation.VERTICAL));
            game.addShip(new Ship(Player.PLAYER_1, new Point(3, 6), 1, Orientation.HORIZONTAL));

            game.addMove(new Move(Player.PLAYER_2, new Point(2, 2)));
            game.addMove(new Move(Player.PLAYER_2, new Point(3, 2)));

            game.addMove(new Move(Player.PLAYER_2, new Point(3, 4)));
            game.addMove(new Move(Player.PLAYER_2, new Point(4, 4)));
            game.addMove(new Move(Player.PLAYER_2, new Point(5, 4)));

            game.addMove(new Move(Player.PLAYER_2, new Point(7, 2)));
            game.addMove(new Move(Player.PLAYER_2, new Point(7, 3)));
            game.addMove(new Move(Player.PLAYER_2, new Point(7, 4)));
            game.addMove(new Move(Player.PLAYER_2, new Point(7, 5)));

            if (game.hasPlayerWon(Player.PLAYER_2) != false)
            {
                throw new Exception("HasPlayerWon TEST 1 FAILED");
            }

            game.addMove(new Move(Player.PLAYER_2, new Point(3, 6)));

            if (game.hasPlayerWon(Player.PLAYER_2) != true)
            {
                throw new Exception("HasPlayerWon TEST 2 FAILED");
            }
        }
        static void TestShip()
        {
            Ship ship1 = new Ship(Player.PLAYER_1, new Point(2, 2), 3, Orientation.HORIZONTAL);
            Point[] ship1points = ship1.GetPoints();
            if (!ship1points[0].SameAs(new Point(2, 2)) ||
                !ship1points[1].SameAs(new Point(3, 2)) ||
                !ship1points[2].SameAs(new Point(4, 2)))
            {
                throw new Exception("Ship points test 1 FAILED");
            }
            Ship ship2 = new Ship(Player.PLAYER_1, new Point(2, 2), 3, Orientation.VERTICAL);
            Point[] ship2points = ship2.GetPoints();
            if (!ship2points[0].SameAs(new Point(2, 2)) ||
                !ship2points[1].SameAs(new Point(2, 3)) ||
                !ship2points[2].SameAs(new Point(2, 4)))
            {
                throw new Exception("Ship points test 2 FAILED");
            }
        }
    }
}
