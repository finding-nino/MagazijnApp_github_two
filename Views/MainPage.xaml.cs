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
		// Constructor \/
		InitializeComponent();
		_databaseService = Dbservice;
		_userSession = userSession;
		UpdateLoginLogoutButton();


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
		RefreshUI(); 
		
		InventoryBtn.IsVisible = _userSession.IsAdmin;
	}

	private async void OnLogOutClicked(object sender, EventArgs e)
	{
		bool confirm = await DisplayAlert("Confirm", "Are you sure you want to log out?", "Yes", "No");

		if (confirm)
		{
			_userSession.LogOut();
			await DisplayAlert("Success", "Logged out successfully", "OK");
			RefreshUI();
			await Shell.Current.GoToAsync("//login"); // '//' absolute navigation
		}
	}

	private void UpdateLoginLogoutButton()
	{
		// Remove all handlers first to prevent duplicates (ea: directing to login 3x on clicking login)
		LoginLogoutBtn.Clicked -= OnLoginClicked;
		LoginLogoutBtn.Clicked -= OnLogOutClicked;

		if (string.IsNullOrEmpty(_userSession.CurrentUsername))
		{
			// No one logged in - show Login button
			LoginLogoutBtn.Text = "Login";
			LoginLogoutBtn.Clicked -= OnLogOutClicked;
			LoginLogoutBtn.Clicked += OnLoginClicked;
		}
		else
		{
			// Someone logged in - show Logout 
			LoginLogoutBtn.Text = "Logout";
			LoginLogoutBtn.Clicked -= OnLoginClicked; 
			LoginLogoutBtn.Clicked += OnLogOutClicked; 
		}
	}

	private void RefreshUI() // One method to handle refreshing UI changes so appropriate buttons show in correct context
    {
		UpdateLoginLogoutButton();

		bool IsAdmin = _userSession.IsAdmin;
		InventoryBtn.IsVisible = _userSession.IsAdmin;
		// Refresh UI based on current session state / and pressing back wont log you out anymore
		
    }
}




//dotnet run --framework net9.0-windows10.0.19041.0
