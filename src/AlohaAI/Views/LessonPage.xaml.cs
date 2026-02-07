using AlohaAI.Helpers;

namespace AlohaAI.Views;

public partial class LessonPage : ContentPage
{
    public LessonPage(ViewModels.LessonViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModels.LessonViewModel.MarkdownContent) && sender is ViewModels.LessonViewModel vm)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ContentArea.Children.Clear();
                if (!string.IsNullOrEmpty(vm.MarkdownContent))
                {
                    try
                    {
                        var rendered = MarkdownRenderer.Render(vm.MarkdownContent);
                        ContentArea.Children.Add(rendered);
                    }
                    catch (Exception ex)
                    {
                        ContentArea.Children.Add(new Label
                        {
                            Text = vm.MarkdownContent,
                            FontSize = 15,
                            Padding = new Thickness(0, 8)
                        });
                        System.Diagnostics.Debug.WriteLine($"Markdown render error: {ex.Message}");
                    }
                }
            });
        }
        else if (e.PropertyName == nameof(ViewModels.LessonViewModel.IsCompleted) && sender is ViewModels.LessonViewModel vm2 && vm2.IsCompleted)
        {
            MainThread.BeginInvokeOnMainThread(async () => await ShowXpPopupAsync(vm2.LessonXp));
        }
    }

    private async Task ShowXpPopupAsync(int xp)
    {
        XpPopupLabel.Text = $"+{xp} XP";
        XpPopup.IsVisible = true;
        XpPopup.Opacity = 0;
        XpPopup.Scale = 0.5;
        XpPopup.TranslationY = 0;

        await Task.WhenAll(
            XpPopup.FadeToAsync(1, 300, Easing.CubicOut),
            XpPopup.ScaleToAsync(1.2, 300, Easing.SpringOut));

        await XpPopup.ScaleToAsync(1, 150, Easing.CubicIn);
        await Task.Delay(800);

        await Task.WhenAll(
            XpPopup.FadeToAsync(0, 400, Easing.CubicIn),
            XpPopup.TranslateToAsync(0, -60, 400, Easing.CubicIn));

        XpPopup.IsVisible = false;
    }
}
