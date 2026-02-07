namespace AlohaAI.Views;

public partial class PathDetailPage : ContentPage
{
    public PathDetailPage(ViewModels.PathDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
