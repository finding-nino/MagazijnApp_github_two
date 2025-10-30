using MagazijnApp.Models;
using MagazijnApp.Services;

namespace MagazijnApp.Views;


public partial class LoginPage : ContentPage
{
    int count = 0;
    DatabaseService _databaseService;

    public LoginPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();

    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UserNameEntry.Text;
        string password = PasswordEntry.Text;
        bool succes = await _databaseService.LoginAsync(username, password);

        if (succes)
        {
            await DisplayAlert("Succes", "Logged in succesfully", "OK");
            await Shell.Current.GoToAsync("main");
        }
        else
        {
            await DisplayAlert("Error", "Login failed", "OK");
        }

    }
    
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = UserNameEntry.Text;
        string password = PasswordEntry.Text;
        bool succes = await _databaseService.RegisterUserAsync(username, password);

        if (succes)
        {
            await DisplayAlert("Succes", "Registered succesfully \nYou can now login", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Username already taken", "OK");
        }

    }
}
