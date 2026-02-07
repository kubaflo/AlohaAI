using System.Collections.ObjectModel;
using System.Windows.Input;
using AlohaAI.Services;

namespace AlohaAI.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly IContentService _contentService;
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

    public ObservableCollection<PathProgressItem> PathProgress { get; } = [];
    public ObservableCollection<AchievementItem> Achievements { get; } = [];

    public ICommand LoadDataCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public ProfileViewModel(IContentService contentService, IProgressService progressService, IStreakService streakService)
    {
        _contentService = contentService;
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

            var paths = await _contentService.GetPathsAsync();
            var totalCompleted = 0;
            PathProgress.Clear();

            foreach (var path in paths)
            {
                var modules = await _contentService.GetModulesAsync(path.Id);
                var totalLessons = modules.Sum(m => m.Lessons.Count);
                var completedLessons = await _progressService.GetCompletedLessonCountAsync(path.Id);
                totalCompleted += completedLessons;
                var progress = totalLessons > 0 ? (double)completedLessons / totalLessons : 0;

                PathProgress.Add(new PathProgressItem
                {
                    Title = path.Title,
                    IconImage = path.Id switch
                    {
                        "agentic-ai" => "icon_explore.png",
                        "ml-fundamentals" => "icon_books.png",
                        "ai-in-practice" => "icon_rocket.png",
                        _ => "icon_books.png"
                    },
                    CompletedLessons = completedLessons,
                    TotalLessons = totalLessons,
                    Progress = progress,
                    BarColor = Color.FromArgb(path.Color)
                });
            }

            LessonsCompleted = totalCompleted;

            // Compute achievements
            Achievements.Clear();
            Achievements.Add(new AchievementItem
            {
                Icon = "icon_explore.png",
                Title = "First Steps",
                Description = "Complete your first lesson",
                IsUnlocked = totalCompleted >= 1
            });
            Achievements.Add(new AchievementItem
            {
                Icon = "icon_notebook.png",
                Title = "Curious Mind",
                Description = "Complete 10 lessons",
                IsUnlocked = totalCompleted >= 10
            });
            Achievements.Add(new AchievementItem
            {
                Icon = "icon_trophy.png",
                Title = "Knowledge Seeker",
                Description = "Complete 25 lessons",
                IsUnlocked = totalCompleted >= 25
            });
            Achievements.Add(new AchievementItem
            {
                Icon = "icon_clock.png",
                Title = "Streak Starter",
                Description = "Reach a 3-day streak",
                IsUnlocked = BestStreak >= 3
            });
            Achievements.Add(new AchievementItem
            {
                Icon = "icon_trophy.png",
                Title = "Week Warrior",
                Description = "Reach a 7-day streak",
                IsUnlocked = BestStreak >= 7
            });
            Achievements.Add(new AchievementItem
            {
                Icon = "icon_rocket.png",
                Title = "XP Hunter",
                Description = "Earn 500 XP",
                IsUnlocked = TotalXp >= 500
            });
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

public class PathProgressItem
{
    public string Title { get; set; } = string.Empty;
    public string IconImage { get; set; } = "icon_books.png";
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public double Progress { get; set; }
    public Color BarColor { get; set; } = Colors.Blue;
    public string ProgressText => $"{CompletedLessons}/{TotalLessons}";
}

public class AchievementItem
{
    public string Icon { get; set; } = "icon_trophy.png";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsUnlocked { get; set; }
    public double IconOpacity => IsUnlocked ? 1.0 : 0.3;
    public Color TextColor => IsUnlocked ? Colors.White : Color.FromArgb("#666666");
    public Color DescColor => IsUnlocked ? Color.FromArgb("#AAAAAA") : Color.FromArgb("#444444");
    public string StatusText => IsUnlocked ? "✓" : "🔒";
}
