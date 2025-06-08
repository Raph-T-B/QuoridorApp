using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using QuoridorMaui.Views;

namespace QuoridorMaui.Pages;

public partial class EndPage : ContentPage
{
    public EndPage()
    {
        InitializeComponent();
    }

    private async void Rejouer_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoisirpartiePage());
    }

    private async void Menu_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage());
    }

    private void Quitter_Tapped(object sender, EventArgs e)
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
#elif MACCATALYST
        UIKit.UIApplication.SharedApplication.PerformSelector(new ObjCRuntime.Selector("terminateWithSuccess"), null, 0f);
#endif
    }
}