using QuoridorStub.Stub;
using QuoridorLib.Models;
using Persistence.Persistence;


namespace QuoridorMaui.Pages;

public partial class ChoisirSauvegardePage : ContentPage
{
    public GamePersistence gamePersistence =new();
    public StubLoadManager loadManager = new();
    private Game Item=null;
    private int ItemIndex=0;

    public ChoisirSauvegardePage()
    {

        InitializeComponent();
        string pathGames = Path.Combine(FileSystem.AppDataDirectory,"Games.json");
        loadManager.LoadGames(gamePersistence.LoadGames(pathGames));
        BindingContext = loadManager;
        
    }

    private async void OnButtonPlayClicked(object sender, EventArgs e)
    {
        if (Item != null)
        {
            await Navigation.PushAsync(new PlayingPage( Item));
        }
        else
        {
            await DisplayAlert("Erreur", "Veuillez selectionner une game pour la lancer", "OK");
        }
    }

    void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        Item = args.SelectedItem as Game;
        ItemIndex = args.SelectedItemIndex;
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoisirpartiePage());
    }
}