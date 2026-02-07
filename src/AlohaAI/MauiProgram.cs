using AlohaAI.Services;
using AlohaAI.ViewModels;
using AlohaAI.Views;
using Microsoft.Extensions.Logging;

namespace AlohaAI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Services
		builder.Services.AddSingleton<IContentService, ContentService>();
		builder.Services.AddSingleton<IProgressService, ProgressService>();
		builder.Services.AddSingleton<IStreakService, StreakService>();

		// ViewModels
		builder.Services.AddTransient<HomeViewModel>();
		builder.Services.AddTransient<PathsViewModel>();
		builder.Services.AddTransient<ProfileViewModel>();

		// Views
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<PathsPage>();
		builder.Services.AddTransient<ProfilePage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
