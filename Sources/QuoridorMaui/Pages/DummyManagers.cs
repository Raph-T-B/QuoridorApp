using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace QuoridorMaui.Pages
{
    public class DummyLoadManager : ILoadManager
    {
        public Game LoadGame() => new Game();
        public GameState LoadGameState() => new GameState();
    }

    public class DummySaveManager : ISaveManager
    {
        public void SaveGame(Game game) { }
        public void SaveGameState(GameState gameState) { }
    }
} 