using MagazijnApp.Services;
using MagazijnApp.Models;

namespace MagazijnApp.Views;

public partial class ProductsPage : ContentPage
{
    int count = 0;
    DatabaseService _databaseService;

    public ProductsPage(DatabaseService dbService)
    {
        InitializeComponent();
        _databaseService = dbService;
    }



    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProducts();

        //onAppearing ensures your page alwyas shows current data, refreshes the data everytime page is opened, thus showing newly added products
        //Naming a collectionview source of items allows you to show it in .xaml



        // To debug you can create a label and show a variable in it
        // DebugLabel.Text = $"Loaded {products.Count} products";
    }



    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        //1. Get the button that was clicked: (Button)sender = the thing that triggered this event was a button
        var button = (Button)sender;

        //2. Get the product that belongs to the button, each button has its BindingContext set to the product it displays
        //BindingContext = the data object (Product) that this UI element is displaying
        var productToDelete = (Products)button.BindingContext;

        //3. Confirm with the user:
        bool confirm = await DisplayAlert("Confirm Delete", $"Delete {productToDelete.ProductName}?", "Yes", "No");

        if (confirm)
        {
            //4. Call the database service to delete the product
            bool succes = await _databaseService.DeleteProductAsync(productToDelete);

            if (succes)
            {
                //5. Refresh the list
                await LoadProducts();
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete product", "OK");
            }
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var productToEdit = (Products)button.BindingContext;

        //Show edit dialogue
        string newName = await DisplayPromptAsync("Edit Product", "Product Name:", initialValue: productToEdit.ProductName);
        string newQuantity = await DisplayPromptAsync("Edit Product", "Quantity:", initialValue: productToEdit.Quantity.ToString());
        string newLocation = await DisplayPromptAsync("Edit Product", "Quantity:", initialValue: productToEdit.Location);

        //If user didnt cancel, update the product:
        if (!string.IsNullOrWhiteSpace(newName) &&
            !string.IsNullOrWhiteSpace(newQuantity) &&
            !string.IsNullOrWhiteSpace(newLocation))
        {
            if (int.TryParse(newQuantity, out int quantity))
            {
                productToEdit.ProductName = newName;
                productToEdit.Quantity = quantity;
                productToEdit.Location = newLocation;

                bool succes = await _databaseService.SaveProductAsync(productToEdit);

                if (succes)
                {
                    await LoadProducts();
                    await DisplayAlert("Succes", "Product updated!", "OK");
                }
            }
        }

        
    }



    private async Task LoadProducts()
    {
        var products = await _databaseService.GetAllProductsAsync();
        ProductsCollection.ItemsSource = products;
    }
    //Product loading put into a seperate method for reusability
    //Can call this from multiple areas
}

