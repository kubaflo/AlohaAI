namespace AlohaAI.Views;

public partial class PathsPage : ContentPage
{
    public PathsPage(ViewModels.PathsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
