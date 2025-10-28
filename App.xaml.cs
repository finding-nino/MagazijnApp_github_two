namespace MagazijnApp.Views;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		Routing.RegisterRoute("inventory", typeof(InventoryPage));
		Routing.RegisterRoute("products", typeof(ProductsPage));
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}