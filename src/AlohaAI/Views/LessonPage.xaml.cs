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
    }
}
