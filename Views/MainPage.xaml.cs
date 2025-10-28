namespace MagazijnApp.Views;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}


	private async void OnInventoryClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("inventory");
	}
	private async void OnProductsClicked(object? sender, EventArgs e)
    {
        // Navigate to ProductsPage to view all products
        await Shell.Current.GoToAsync("products");
    }
	

}
