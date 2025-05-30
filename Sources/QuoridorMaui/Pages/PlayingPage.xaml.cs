using QuoridorMaui.Models;

namespace QuoridorMaui.Pages;

public partial class PlayingPage : ContentPage
{
	public GameBoard GameBoard { get; } = new GameBoard();

	public PlayingPage()
	{
		InitializeComponent();
		BindingContext = this;
	}
}