using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuoridorLib.Models
{
    public delegate void Progression(int pourcentage);
    
    public class Round
    {
        private Player CurrentPlayer;
        private readonly Board Board;
        private Game? game;

        public Player CurrentPlayerProperty => CurrentPlayer;

        public Round(Player player, Board board)
        {
            Board = board;
            CurrentPlayer = player;
        }
        public void SwitchCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
        }

        public bool MovePawn(int newX, int newY)
        {
            Position position = new Position(newX, newY);
            bool moved = false;

            if (CurrentPlayer == Board.Pawn1.GetPlayer())
            {
                moved = Board.MovePawn(Board.Pawn1, position);
                if (moved && newX == 8 && game != null)
                {
                    game.GetBestOf().AddPlayer1Victory();
                    Console.WriteLine($"Score mis à jour - Joueur 1: {game.GetBestOf().GetPlayer1Score()}, Joueur 2: {game.GetBestOf().GetPlayer2Score()}");
                    return true;
                }
            }
            else
            {
                moved = Board.MovePawn(Board.Pawn2, position);
                if (moved && newX == 0 && game != null)
                {
                    game.GetBestOf().AddPlayer2Victory();
                    Console.WriteLine($"Score mis à jour - Joueur 1: {game.GetBestOf().GetPlayer1Score()}, Joueur 2: {game.GetBestOf().GetPlayer2Score()}");
                    return true;
                }
            }

            return moved;
        }

        public bool PlacingWall(int x, int y, string orientation)
        {
            if (!Board.IsWallONBoard(x, y, orientation))
            {
                return false;
            }

            List<Position> wallPositions = GetWallPositions(x, y, orientation);

            Wall wall1 = new Wall(wallPositions[0], wallPositions[1]);
            Wall wall2 = new Wall(wallPositions[2], wallPositions[3]);
            
            if (!Board.IsCoupleWallPlaceable(wall1, wall2))
            {
                return false;
            }

            return Board.AddCoupleWall(wall1, wall2, orientation);
        }

        private static List<Position> GetWallPositions(int x, int y, string orientation)
        {
            int x1, y1, x2, y2, x3, y3, x4, y4;
            if (orientation == "vertical")
            {
                x1 = x; y1 = y;
                x2 = x; y2 = y + 1;
                x3 = x + 1; y3 = y;
                x4 = x + 1; y4 = y + 1;
            }
            else // horizontal
            {
                x1 = x; y1 = y;
                x2 = x + 1; y2 = y;
                x3 = x; y3 = y + 1;
                x4 = x + 1; y4 = y + 1;
            }
            Position position1 = new(x1, y1);
            Position position2 = new(x2, y2);
            Position position3 = new(x3, y3);
            Position position4 = new(x4, y4);
            List<Position> wallPositions = [position1, position2, position3, position4];
            return wallPositions;
        }

        public Board GetBoard()
        {
            return Board;
        }

        public void SetGame(Game game)
        {
            this.game = game;
        }

        public Game? GetGame()
        {
            return game;
        }
    }
} 