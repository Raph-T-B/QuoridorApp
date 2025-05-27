namespace QuoridorMaui.Pages;

public partial class HomePage : ContentPage
{
    private async void NouvellePartie_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirpartiepage");
    }
    private async void LeaderBoard_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("leaderboardpage");
    }
    private async void Regles_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("settingpage");
    }
    private async void Quitter_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    public HomePage()
	{
		InitializeComponent();
	}
	private void test_bouton(object sender, EventArgs e){
		DisplayAlert("Confirmer","coucou", "ok");

    }

    private async void Jouer_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirpartiepage");
    }

    private async void Leaderboard_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("leaderboardpage");
    }

    private async void Regles_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("regles");
    }

    private void Quitter_Tapped(object sender, EventArgs e)
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
        System.Windows.Application.Current.Shutdown();
#elif MACCATALYST
        UIKit.UIApplication.SharedApplication.PerformSelector(new ObjCRuntime.Selector("terminateWithSuccess"), null, 0f);
#endif
    }
}