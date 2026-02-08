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
            await Task.Delay(500); // Brief splash display
            await _progressService.InitializeAsync();
            await _progressService.SaveSettingAsync("onboarding_completed", "true");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Loading error: {ex.Message}");
        }
        finally
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current?.Windows.Count > 0)
                {
                    Application.Current.Windows[0].Page = new AppShell();
                }
            });
        }
    }
}
