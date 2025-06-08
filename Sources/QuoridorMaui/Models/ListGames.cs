using System.Collections.ObjectModel;
using QuoridorLib.Models;

namespace QuoridorMaui.Models;

public class ListGames
{
    public ObservableCollection<Game> Games { get; set; } = new();


    public ListGames() { }

    public void Load(List<Game> games)
    {
        Games.Clear();
        foreach (Game g in games)
            Games.Add(g);
    }
}