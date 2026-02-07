using System.Windows.Input;

namespace AlohaAI.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private bool _isDarkMode;
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (SetProperty(ref _isDarkMode, value))
                ApplyTheme();
        }
    }

    public ICommand GoBackCommand { get; }

    public SettingsViewModel()
    {
        Title = "Settings";
        _isDarkMode = Application.Current?.RequestedTheme == AppTheme.Dark;
        GoBackCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }

    private void ApplyTheme()
    {
        if (Application.Current != null)
            Application.Current.UserAppTheme = IsDarkMode ? AppTheme.Dark : AppTheme.Light;
    }
}
