using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Models;

namespace QuoridorLib.Models
{

     class Board
    {
        private Dictionary<string,Pawn> Pawns;
        private List<Wall> Walls;
        private int BoardWith;
        private int BoardHeight;
        /// <summary>
        /// The Board's constructor
        /// </summary>
        protected Board() 
        { 
            Pawns = new Dictionary<string, Pawn>();
            Walls = new List<Wall>();
        }
        /// <summary>
        /// Init the Board as a 1vs1 quoridor Board
        /// </summary>
        /// <param name="player1">Player 1's Name</param>
        /// <param name="player2">Player 2's Name</param>
        /// <param name="positionP1">Player 1's Position</param>
        /// <param name="positionP2">Player 2's Position</param>
        public void Init1vs1QuoridorBoard(string player1, string player2, Position positionP1, Position positionP2)
        {
            Pawn pawnP1 = new(positionP1);
            Pawn pawnP2 = new(positionP2);
            Pawns.Add(player1, pawnP1);
            Pawns.Add(player2, pawnP2);
            BoardHeight = 9;
            BoardWith = 9;
        }
        

        public void AddCoupleWall(Wall wall1,Wall wall2)
        {
            Walls.Add(wall1);
            Walls.Add(wall2);
        }
        
        /// <summary>
        /// Move a Pawn if it's possible -> check if the next position: 
        /// - is on board
        /// - is on a beside case of the pawn case
        /// - is not on another pawn case
        /// - is not a position behind a wall
        /// </summary>
        /// <param name="pawnName">The pawn Name</param>
        /// <param name="position">The position where the pawn will go</param>
        /// <returns>True if the Pawn moved, false if not</returns>
        public bool MovePawn(string pawnName,Position position)
        {
            Pawn pawn = Pawns[pawnName];
            if (IsPawnOnBoard(position) &&
                IsCaseBeside(pawn,position) &&
                !IsOnAPawnCase(position) &&
                !IsWallbetween(pawn,position) )
            {
                Pawns[pawnName].Move(position);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if a Wall is between the Pawn's position and the Position theCase
        /// </summary>
        /// <param name="pawn">The pawn to check</param>
        /// <param name="theCase">The Position to check</param>
        /// <returns>True if a Wall is between, false if not</returns>
        private bool IsWallbetween(Pawn pawn,Position theCase)
        {
            foreach (Wall wall in Walls)
            {
                Position wallFirstP = wall.GetFirstPosition();
                Position wallSecondP = wall.GetSecondPosition();
                Position pawnPosition = pawn.GetPosition();

                if (wallFirstP == theCase ||
                    wallSecondP == pawnPosition)
                {
                    return true;
                }

                if (wallFirstP == pawnPosition ||
                    wallSecondP == theCase)
                { 
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Check if a Pawn is on the Position position
        /// </summary>
        /// <param name="theCase">The position to check</param>
        /// <returns>True if a Pawn is on the case, false if not </returns>
        private bool IsOnAPawnCase( Position theCase) 
        { 
            foreach ((string name,Pawn pawn) in Pawns) 
            {
                if (pawn.GetPawnPosition() == theCase)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Check if the position is on a beside case of the pawn position
        /// </summary>
        /// <param name="pawn">The pawn itself</param>
        /// <param name="theCase">The case to check</param>
        /// <returns>True if the case is beside, false if not</returns>
        private static bool IsCaseBeside(Pawn pawn,Position theCase) 
        {
            int xPawn = pawn.GetPositionX();
            int yPawn = pawn.GetPositionY();
            int xNew= theCase.GetPositionX();
            int yNew= theCase.GetPositionY();

            if (pawn.GetPosition() == theCase)
            {
                return false;
            }
            if (xPawn == xNew &&
                (yPawn - yNew == 1 || yPawn - yNew == -1)) 
            { 
                return true; 
            }

            if (yPawn == yNew &&
                (xPawn - xNew == 1 || xPawn - xNew == -1))
            { 
                return true;
            }
            return false;
        }
        /// <summary>
        /// Check if the position is on the Board
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>True if position in the Board, false if not</returns>
        private bool IsPawnOnBoard(Position position)
        {
            int x = position.GetPositionX();
            int y = position.GetPositionY();
            if (x<=BoardWith && x>=0)
                if (y<=BoardHeight && y>=0)
                    return true;
            return false;
        }
        /// <summary>
        /// Check if the given Position is correct to place the wall
        /// </summary>
        /// <param name="x">The x origin of wall's position</param>
        /// <param name="y">The y origin of wall's position</param>
        /// <param name="orientation">The wall orientation</param>
        /// <returns>True if the position is correct, false if not</returns>
        public static bool IsWallONBoard(int x,int y,string orientation)
        {
            if (orientation == "vertical")
            {
                if ( x >= 0 && x <=8
                    && y >= 0 && y <= 7 )
                    return true;
            }
            else //horizontal
            {
                if (x >= 0 && x <= 7
                    && y >= 0 && y <= 8)
                    return true;
            }
            return false;
            // à mettre dans la classe game  :
            //public event EventHandler<T> BoardChanged;
            // à mettre dans la classe game  :
            /*public class BoardChangedEventArgs : EventArgs
            {
                public Board Board { get; private init; }

                public BoardChangedEventArgs()
            }
            */
        }


    }
    
}
