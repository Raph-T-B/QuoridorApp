using QuoridorMaui.Pages;

namespace QuoridorMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("homepage", typeof(homepage));
        }
    }
}
