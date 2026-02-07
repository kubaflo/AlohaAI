namespace AlohaAI.Views;

public partial class HomePage : ContentPage
{
    public HomePage(ViewModels.HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
