namespace MagazijnApp.Views;
using MagazijnApp.Models;
using MagazijnApp.Services;

public partial class MainPage : ContentPage
{
	int count = 0;
	DatabaseService _databaseService;
	UserSession _userSession;

	public MainPage(DatabaseService Dbservice, UserSession userSession)
	{
		InitializeComponent();
		_databaseService = Dbservice;
		_userSession = userSession;

		if (_userSession.IsAdmin)
		{
			InventoryBtn.IsVisible = true;
		}
		else
		{
			InventoryBtn.IsVisible = false;
		}
	}


	private async void OnInventoryClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("inventory");
	}
	private async void OnProductsClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("products");
	}
	private async void OnLoginClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("login");
	}

protected override void OnAppearing()
    {
		base.OnAppearing();
		// Refresh UI based on current session state / AKA pressing back wont log you out anymore
		InventoryBtn.IsVisible = _userSession.IsAdmin;
    }



}

//dotnet run --framework net9.0-windows10.0.19041.0
