using QuoridorMaui.Views;

namespace QuoridorMaui.Pages;

public partial class SettingPage : ContentPage
{
    private bool isDarkTheme = true;

    public SettingPage()
    {
        InitializeComponent();
        UpdateThemeButtons();
    }

    private void OnThemeButtonClicked(object sender, EventArgs e)
    {
        if (sender is Bouton button)
        {
            isDarkTheme = button.Texte == "Sombre";
            UpdateThemeButtons();
            // TODO: Implémenter le changement de thème
        }
    }

    private void UpdateThemeButtons()
    {
        BtnSombre.BackgroundColor = isDarkTheme ? Colors.Gray : Colors.Transparent;
        BtnClair.BackgroundColor = !isDarkTheme ? Colors.Gray : Colors.Transparent;
    }

    private void VolumeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        // TODO: Implémenter le changement de volume
    }

    private void MusicSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        // TODO: Implémenter l'activation/désactivation de la musique
    }

    private void SoundEffectsSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        // TODO: Implémenter l'activation/désactivation des effets sonores
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}