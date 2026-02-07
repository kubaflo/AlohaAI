namespace AlohaAI.Views;

public partial class PathDetailPage : ContentPage
{
    public PathDetailPage(ViewModels.PathDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        MainContent.Opacity = 0;
        MainContent.TranslationY = 30;
        await Task.WhenAll(
            MainContent.FadeToAsync(1, 400, Easing.CubicOut),
            MainContent.TranslateToAsync(0, 0, 400, Easing.CubicOut));
    }
}
