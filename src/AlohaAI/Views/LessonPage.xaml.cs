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
            ContentArea.Children.Clear();
            if (!string.IsNullOrEmpty(vm.MarkdownContent))
            {
                var rendered = MarkdownRenderer.Render(vm.MarkdownContent);
                ContentArea.Children.Add(rendered);
            }
        }
    }
}
