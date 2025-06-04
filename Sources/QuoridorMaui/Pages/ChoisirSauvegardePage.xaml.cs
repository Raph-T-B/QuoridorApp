using QuoridorLib.Interfaces;
using QuoridorLib.Managers;
using QuoridorLib.Models;

namespace QuoridorMaui.Pages;

public partial class ChoisirSauvegardePage : ContentPage
{

    public StubLoadManager loadManager = new();
    public int BoGlobal;

    public ChoisirSauvegardePage()
    {
        InitializeComponent();
        loadManager.LoadGames();
        BindingContext = loadManager;
        
    }
}