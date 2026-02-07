using System.Windows.Input;
using AlohaAI.Services;

namespace AlohaAI.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly IProgressService _progressService;
    private readonly IStreakService _streakService;

    private int _totalXp;
    public int TotalXp
    {
        get => _totalXp;
        set => SetProperty(ref _totalXp, value);
    }

    private int _currentStreak;
    public int CurrentStreak
    {
        get => _currentStreak;
        set => SetProperty(ref _currentStreak, value);
    }

    private int _bestStreak;
    public int BestStreak
    {
        get => _bestStreak;
        set => SetProperty(ref _bestStreak, value);
    }

    private int _lessonsCompleted;
    public int LessonsCompleted
    {
        get => _lessonsCompleted;
        set => SetProperty(ref _lessonsCompleted, value);
    }

    private int _level;
    public int Level
    {
        get => _level;
        set => SetProperty(ref _level, value);
    }

    public ICommand LoadDataCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public ProfileViewModel(IProgressService progressService, IStreakService streakService)
    {
        _progressService = progressService;
        _streakService = streakService;
        Title = "Profile";

        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        OpenSettingsCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync("settings"));
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            TotalXp = await _progressService.GetTotalXpAsync();
            CurrentStreak = await _streakService.GetCurrentStreakAsync();
            BestStreak = await _streakService.GetBestStreakAsync();
            Level = TotalXp / 100 + 1;

            var agenticCount = await _progressService.GetCompletedLessonCountAsync("agentic-ai");
            var mlCount = await _progressService.GetCompletedLessonCountAsync("ml-fundamentals");
            var practiceCount = await _progressService.GetCompletedLessonCountAsync("ai-in-practice");
            LessonsCompleted = agenticCount + mlCount + practiceCount;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading profile: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
