using MagazijnApp.Services;
using Microsoft.Extensions.Logging;

namespace MagazijnApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<Views.App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<DatabaseService>(); // Add it here instead of adding it manually on each page.
		builder.Services.AddSingleton<UserSession>();
		//AddSingleton = create one instance for the entire app

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}


//dotnet run -f net9.0-maccatalyst