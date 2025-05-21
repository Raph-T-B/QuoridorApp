using QuoridorMaui.Pages;  

namespace QuoridorMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("choisirpartiepage", typeof(ChoisirpartiePage));
            Routing.RegisterRoute("choisirsauvegardepage", typeof(ChoisirSauvegardePage));
            Routing.RegisterRoute("endpage", typeof(EndPage));
            Routing.RegisterRoute("homepage", typeof(HomePage));
            Routing.RegisterRoute("leaderboardpage", typeof(LeaderBoardPage));
            Routing.RegisterRoute("page1vs1", typeof(Page1VS1));
            Routing.RegisterRoute("page1vsbot", typeof(Page1VSBot));
            Routing.RegisterRoute("pausepage", typeof(PausePage));
            Routing.RegisterRoute("playingpage", typeof(PlayingPage));
            Routing.RegisterRoute("regles", typeof(Regles));
            Routing.RegisterRoute("settingpage", typeof(SettingPage));
        }
    }
}
