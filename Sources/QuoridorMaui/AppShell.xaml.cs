using QuoridorMaui.Pages;

namespace QuoridorMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Page1VSBot), typeof(Page1VSBot));
        }
    }
}
