using QuoridorLib.Managers;

namespace QuoridorMaui.Pages;

public partial class ChoisirSauvegardePage : ContentPage
{


    

    public ChoisirSauvegardePage()
    {
        InitializeComponent();
        StubLoadManager newGame= new();
        newGame.LoadPlayers();
        newGame.LoadGame();
        
    }
}