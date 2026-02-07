using System.Collections.ObjectModel;
using System.Windows.Input;
using AlohaAI.Services;

namespace AlohaAI.ViewModels;

public class HomePathItem
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconEmoji { get; set; } = "📘";
    public Color Color { get; set; } = Colors.Blue;
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public double Progress { get; set; }
    public string ProgressText => $"{CompletedLessons}/{TotalLessons} Lessons";
}

public class HomeViewModel : BaseViewModel
{
    private readonly IContentService _contentService;
    private readonly IProgressService _progressService;
    private readonly IStreakService _streakService;

    public ObservableCollection<HomePathItem> PathItems { get; } = [];

    private int _currentStreak;
    public int CurrentStreak
    {
        get => _currentStreak;
        set => SetProperty(ref _currentStreak, value);
    }

    private int _totalXp;
    public int TotalXp
    {
        get => _totalXp;
        set => SetProperty(ref _totalXp, value);
    }

    private int _todayLessons;
    public int TodayLessons
    {
        get => _todayLessons;
        set => SetProperty(ref _todayLessons, value);
    }

    private string _continuePathId = string.Empty;
    public string ContinuePathId
    {
        get => _continuePathId;
        set => SetProperty(ref _continuePathId, value);
    }

    private string _continuePathTitle = string.Empty;
    public string ContinuePathTitle
    {
        get => _continuePathTitle;
        set => SetProperty(ref _continuePathTitle, value);
    }

    private bool _hasContinue;
    public bool HasContinue
    {
        get => _hasContinue;
        set => SetProperty(ref _hasContinue, value);
    }

    private double _dailyProgress;
    public double DailyProgress
    {
        get => _dailyProgress;
        set => SetProperty(ref _dailyProgress, value);
    }

    private bool _dailyGoalMet;
    public bool DailyGoalMet
    {
        get => _dailyGoalMet;
        set => SetProperty(ref _dailyGoalMet, value);
    }

    public ICommand LoadDataCommand { get; }
    public ICommand NavigateToPathsCommand { get; }
    public ICommand ContinueLearningCommand { get; }
    public ICommand NavigateToPathCommand { get; }

    public HomeViewModel(IContentService contentService, IProgressService progressService, IStreakService streakService)
    {
        _contentService = contentService;
        _progressService = progressService;
        _streakService = streakService;
        Title = "Home";

        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        NavigateToPathsCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync("//paths"));
        NavigateToPathCommand = new AsyncRelayCommand<string>(async pathId =>
        {
            if (!string.IsNullOrEmpty(pathId))
                await Shell.Current.GoToAsync($"pathdetail?pathId={pathId}");
        });
        ContinueLearningCommand = new AsyncRelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(ContinuePathId))
                await Shell.Current.GoToAsync($"pathdetail?pathId={ContinuePathId}");
        });
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            await _progressService.InitializeAsync();
            CurrentStreak = await _streakService.GetCurrentStreakAsync();
            TotalXp = await _progressService.GetTotalXpAsync();
            TodayLessons = await _streakService.GetTodayLessonsCountAsync();
            DailyProgress = Math.Min(1.0, TodayLessons / 3.0);
            DailyGoalMet = TodayLessons >= 3;

            var lastLesson = await _progressService.GetLastCompletedLessonAsync();
            if (lastLesson != null)
            {
                ContinuePathId = lastLesson.PathId;
                var path = await _contentService.GetPathAsync(lastLesson.PathId);
                ContinuePathTitle = path?.Title ?? "Continue Learning";
                HasContinue = true;
            }

            // Load path items with progress
            var paths = await _contentService.GetPathsAsync();
            PathItems.Clear();
            foreach (var path in paths)
            {
                var modules = await _contentService.GetModulesAsync(path.Id);
                var totalLessons = modules.Sum(m => m.Lessons.Count);
                var progress = await _progressService.GetPathProgressAsync(path.Id, totalLessons);
                var completedLessons = await _progressService.GetCompletedLessonCountAsync(path.Id);

                PathItems.Add(new HomePathItem
                {
                    Id = path.Id,
                    Title = path.Title,
                    Description = path.Description,
                    IconEmoji = path.Id switch
                    {
                        "agentic-ai" => "🤖",
                        "ml-fundamentals" => "🧠",
                        "ai-in-practice" => "🚀",
                        _ => "📘"
                    },
                    Color = Color.FromArgb(path.Color),
                    CompletedLessons = completedLessons,
                    TotalLessons = totalLessons,
                    Progress = progress
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading home data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
