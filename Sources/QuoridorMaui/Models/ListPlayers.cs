using System.Collections.ObjectModel;
using QuoridorLib.Models;

namespace QuoridorMaui.Models
{
    public class ListPlayers
    {
        public ObservableCollection<Player> Players { get; set; } = new();


        public ListPlayers() { }

        public void Load(List<Player> players)
        {
            Players.Clear();
            foreach (Player p in players)
                Players.Add(p);
        }
    }
}
