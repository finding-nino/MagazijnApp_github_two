using MagazijnApp.Models;
using MagazijnApp.Services;

namespace MagazijnApp.Views;


public partial class LoginPage : ContentPage
{
    int count = 0;
    DatabaseService _databaseService;
    UserSession _userSession;

    public LoginPage(DatabaseService dbService, UserSession userSession)
    {
        InitializeComponent();
        _databaseService = dbService;
        _userSession = userSession;

    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UserNameEntry.Text;
        string password = PasswordEntry.Text;
        bool succes = await _databaseService.LoginAsync(username, password);

        if (succes)
        {
            _userSession.CurrentUsername = username;
            if (username == "123456")
            {
                _userSession.CurrentRole = "admin";
            }
            else
            {
                _userSession.CurrentRole = "regular";
            }


            await DisplayAlert("Succes", $"Logged in succesfully, your role is {_userSession.CurrentRole}", "OK");
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

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Controleer of beide velden zijn ingevuld
        bool areFieldsFilled = !string.IsNullOrWhiteSpace(UserNameEntry.Text) &&
                              !string.IsNullOrWhiteSpace(PasswordEntry.Text);

        // Zet beide knoppen enabled/disabled
        LoginButton.IsEnabled = areFieldsFilled;
        RegisterButton.IsEnabled = areFieldsFilled;

        UpdateButtonColors(areFieldsFilled);

    }

    private void UpdateButtonColors(bool areFieldsFilled)
{
    Color enabledColor = Color.FromArgb("#E46E56");
    Color disabledColor = Color.FromArgb("#666666");
    
    LoginButton.BackgroundColor = areFieldsFilled ? enabledColor : disabledColor;
    RegisterButton.BackgroundColor = areFieldsFilled ? enabledColor : disabledColor;
}
    
}
