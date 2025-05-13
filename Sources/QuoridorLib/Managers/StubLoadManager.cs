using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace QuoridorLib.Managers
{
    public class StubLoadManager : ILoadManager
    {
        public Game LoadGame()
        {
            return new Game();
        }

        public GameState LoadGameState()
        {
            return new GameState();
        }
    }
}
