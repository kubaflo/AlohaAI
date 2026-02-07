namespace AlohaAI.Views;

public partial class QuizPage : ContentPage
{
    public QuizPage(ViewModels.QuizViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
