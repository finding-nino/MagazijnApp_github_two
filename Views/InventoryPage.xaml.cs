using System.Net.Http.Headers;
using System.Threading.Tasks;
using MagazijnApp.Models;
using MagazijnApp.Services;
namespace MagazijnApp.Views;


public partial class InventoryPage : ContentPage
{
    // DatabaseService _databaseService; // Database service for saving products
    // Moved to MauiProgram.cs to prevent creating multiple instances
    DatabaseService _databaseService;

    public InventoryPage(DatabaseService dbService)
    {
        InitializeComponent();
        _databaseService = dbService; // Initialize database service
    }

    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        // Get values from form entries
        string productName = ProductNameEntry.Text;
        string quantityAmount = QuantityEntry.Text;
        string locationPlace = LocationEntry.Text;

        // Validate quantity is a number
        if (!int.TryParse(quantityAmount, out int quantity))
        {
            await DisplayAlert("Error", "Please enter a valid number for quantity", "OK");
            return;
        }

        // Create new product object
        Products newProduct = new Products();
        newProduct.ProductName = productName;
        newProduct.Quantity = quantity;
        newProduct.Location = locationPlace;

        // Save to database
        bool success = await _databaseService.SaveProductAsync(newProduct);
        
        if (success)
        {
            await DisplayAlert("Success", 
                $"Product created:\nName: {newProduct.ProductName}\nQuantity: {newProduct.Quantity}\nLocation: {newProduct.Location}", 
                "OK");
            
            // Optional: Clear form after successful save
            ProductNameEntry.Text = string.Empty;
            QuantityEntry.Text = string.Empty;
            LocationEntry.Text = string.Empty;
        }
        else
        {
            await DisplayAlert("Error", "Failed to create product", "OK");
        }
    }

    private async void OnProductsClicked(object? sender, EventArgs e)
    {
        // Navigate to ProductsPage to view all products
        await Shell.Current.GoToAsync("products");
    }
}