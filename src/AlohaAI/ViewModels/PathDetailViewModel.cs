using System.Collections.ObjectModel;
using System.Windows.Input;
using AlohaAI.Models;
using AlohaAI.Services;

namespace AlohaAI.ViewModels;

[QueryProperty(nameof(PathId), "pathId")]
public class PathDetailViewModel : BaseViewModel
{
    private readonly IContentService _contentService;
    private readonly IProgressService _progressService;

    private string _pathId = string.Empty;
    public string PathId
    {
        get => _pathId;
        set { SetProperty(ref _pathId, value); LoadDataCommand.Execute(null); }
    }

    private string _pathDescription = string.Empty;
    public string PathDescription
    {
        get => _pathDescription;
        set => SetProperty(ref _pathDescription, value);
    }

    private Color _pathColor = Colors.Blue;
    public Color PathColor
    {
        get => _pathColor;
        set => SetProperty(ref _pathColor, value);
    }

    public ObservableCollection<ModuleDisplayItem> Modules { get; } = [];

    public ICommand LoadDataCommand { get; }
    public ICommand SelectLessonCommand { get; }
    public ICommand StartQuizCommand { get; }

    public PathDetailViewModel(IContentService contentService, IProgressService progressService)
    {
        _contentService = contentService;
        _progressService = progressService;

        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        SelectLessonCommand = new AsyncRelayCommand<LessonDisplayItem>(async lesson =>
        {
            if (lesson != null)
                await Shell.Current.GoToAsync($"lesson?pathId={PathId}&moduleId={lesson.ModuleId}&lessonId={lesson.Id}");
        });
        StartQuizCommand = new AsyncRelayCommand<string>(async moduleId =>
        {
            if (!string.IsNullOrEmpty(moduleId))
                await Shell.Current.GoToAsync($"quiz?pathId={PathId}&moduleId={moduleId}");
        });
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy || string.IsNullOrEmpty(PathId)) return;
        IsBusy = true;

        try
        {
            var path = await _contentService.GetPathAsync(PathId);
            if (path != null)
            {
                Title = path.Title;
                PathDescription = path.Description;
                PathColor = Color.FromArgb(path.Color);
            }

            var modules = await _contentService.GetModulesAsync(PathId);
            Modules.Clear();

            foreach (var module in modules)
            {
                var lessons = new ObservableCollection<LessonDisplayItem>();
                foreach (var lesson in module.Lessons)
                {
                    var completed = await _progressService.IsLessonCompletedAsync(PathId, module.Id, lesson.Id);
                    lessons.Add(new LessonDisplayItem
                    {
                        Id = lesson.Id,
                        ModuleId = module.Id,
                        Title = lesson.Title,
                        Xp = lesson.Xp,
                        IsCompleted = completed
                    });
                }

                var completedCount = lessons.Count(l => l.IsCompleted);
                Modules.Add(new ModuleDisplayItem
                {
                    Id = module.Id,
                    Title = module.Title,
                    Description = module.Description,
                    Lessons = lessons,
                    CompletedCount = completedCount,
                    TotalCount = lessons.Count,
                    HasQuiz = !string.IsNullOrEmpty(module.Quiz)
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading path detail: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}

public class ModuleDisplayItem
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservableCollection<LessonDisplayItem> Lessons { get; set; } = [];
    public int CompletedCount { get; set; }
    public int TotalCount { get; set; }
    public bool HasQuiz { get; set; }
    public string ProgressText => $"{CompletedCount}/{TotalCount}";
    public bool AllLessonsComplete => CompletedCount == TotalCount && TotalCount > 0;
}

public class LessonDisplayItem
{
    public string Id { get; set; } = string.Empty;
    public string ModuleId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Xp { get; set; }
    public bool IsCompleted { get; set; }
    public string StatusIcon => IsCompleted ? "✅" : "○";
}
