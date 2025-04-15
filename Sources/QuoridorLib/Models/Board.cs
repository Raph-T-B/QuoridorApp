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
        public void InitQuoridorBoard(int nbPlayers, List<string> playersNames)
        {

            List<(int,int)> positions = [(0,4),(8,4)];
            for (int i = 0; i < nbPlayers; i++)
            {
                Pawn pawn = new Pawn(positions[i][0], positions[i][1]);
            }
        }
        /// <summary>
        /// Add a pawn to the Pawn's list
        /// </summary>
        /// <param name="name">The Pawn Name</param>
        /// <param name="pawn">The Pawn to add</param>
        private void AddPawn(string name,Pawn pawn)
        {
            Pawns[name]=pawn;
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
        private bool MovePawn(string pawnName,Position position)
        {
            Pawn pawn = Pawns[pawnName];
            if (IsOnBoard(position) & IsCaseBeside(pawn,position) & !IsOnAPawnCase(position) & !IsWallbetween(pawn,position) )
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
                if (wall.GetFirstPosition() == theCase || wall.GetSecondPosition() == pawn.GetPosition())return true;
                if (wall.GetFirstPosition() == pawn.GetPosition() || wall.GetSecondPosition() == theCase) return true;
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
            if (pawn.GetPosition() == theCase) return false;
            if (xPawn == xNew && ( yPawn-yNew==1 || yPawn-yNew==-1))return true;
            if (yPawn == yNew && (xPawn - xNew == 1 || xPawn - xNew == -1)) return true;
            return false;
        }
        /// <summary>
        /// Check if the position is on the Board
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>True if position in the Board, false if not</returns>
        private bool IsOnBoard(Position position)
        {
            int x = position.GetPositionX();
            int y = position.GetPositionY();
            if (x<=BoardWith & x>=0)
                if (y<=BoardHeight & y>=0)
                    return true;
            return false;
        }
        
    }
    
}
