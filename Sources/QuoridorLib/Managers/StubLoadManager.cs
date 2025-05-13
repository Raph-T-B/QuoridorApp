using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Models;

namespace QuoridorLib.Managers
{
    internal class StubLoadManager : ILoadManager
    {
        public Board LoadBoard()
        {
            Board board = new Board();
            board.AddPawn();
            board.AddPawn();
            return board;

        }
    }
}
