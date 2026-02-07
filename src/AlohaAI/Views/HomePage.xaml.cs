namespace AlohaAI.Views;

public partial class HomePage : ContentPage
{
    public HomePage(ViewModels.HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewModels.HomeViewModel vm)
            vm.LoadDataCommand.Execute(null);
    }
}
