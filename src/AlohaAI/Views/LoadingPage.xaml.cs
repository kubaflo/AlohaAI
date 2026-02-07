using AlohaAI.Services;

namespace AlohaAI.Views;

public partial class LoadingPage : ContentPage
{
    private readonly IProgressService _progressService;

    public LoadingPage(IProgressService progressService)
    {
        InitializeComponent();
        _progressService = progressService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _progressService.InitializeAsync();
            var onboarded = await _progressService.GetSettingAsync("onboarding_completed");

            if (Application.Current?.Windows.Count > 0)
            {
                if (onboarded == "true")
                    Application.Current.Windows[0].Page = new AppShell();
                else
                    Application.Current.Windows[0].Page = new OnboardingPage(_progressService);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Loading error: {ex.Message}");
            if (Application.Current?.Windows.Count > 0)
                Application.Current.Windows[0].Page = new AppShell();
        }
    }
}
