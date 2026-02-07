namespace AlohaAI.Views;

public partial class PathsPage : ContentPage
{
    public PathsPage(ViewModels.PathsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewModels.PathsViewModel vm)
            vm.LoadPathsCommand.Execute(null);
    }
}
