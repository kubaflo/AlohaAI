using AlohaAI.Services;

namespace AlohaAI.Views;

public partial class OnboardingPage : ContentPage
{
    private readonly IProgressService _progressService;

    public OnboardingPage(IProgressService progressService)
    {
        InitializeComponent();
        _progressService = progressService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Opacity = 0;
        await this.FadeToAsync(1, 500, Easing.CubicOut);
    }

    private async void OnGetStartedClicked(object? sender, EventArgs e)
    {
        await _progressService.SaveSettingAsync("onboarding_completed", "true");

        if (Application.Current?.Windows.Count > 0)
            Application.Current.Windows[0].Page = new AppShell();
    }
}
