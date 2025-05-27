using QuoridorLib.Models;

namespace QuoridorMaui.Pages;

public partial class LeaderBoardPage : ContentPage
{
    public List<Player> Players { get; set; }

    public LeaderBoardPage()
    {
        InitializeComponent();

        Players =
        [
            new Player("Jojo"),
            new Player("Moule"),
            new Player("Gab1"),
            new Player("Raph"),
            new Player("Jojo"),
            new Player("Moule"),
            new Player("Gab1"),
            new Player("Raph")
        ];

        BindingContext = this;
    }
}
