using System.Windows.Input;
using AlohaAI.Services;

namespace AlohaAI.ViewModels;

[QueryProperty(nameof(PathId), "pathId")]
[QueryProperty(nameof(ModuleId), "moduleId")]
[QueryProperty(nameof(LessonId), "lessonId")]
public class LessonViewModel : BaseViewModel
{
    private readonly IContentService _contentService;
    private readonly IProgressService _progressService;
    private readonly IStreakService _streakService;

    private string _pathId = string.Empty;
    public string PathId
    {
        get => _pathId;
        set => SetProperty(ref _pathId, value);
    }

    private string _moduleId = string.Empty;
    public string ModuleId
    {
        get => _moduleId;
        set => SetProperty(ref _moduleId, value);
    }

    private string _lessonId = string.Empty;
    public string LessonId
    {
        get => _lessonId;
        set { SetProperty(ref _lessonId, value); LoadLessonCommand.Execute(null); }
    }

    private string _markdownContent = string.Empty;
    public string MarkdownContent
    {
        get => _markdownContent;
        set => SetProperty(ref _markdownContent, value);
    }

    private bool _isCompleted;
    public bool IsCompleted
    {
        get => _isCompleted;
        set => SetProperty(ref _isCompleted, value);
    }

    private int _lessonXp;
    public int LessonXp
    {
        get => _lessonXp;
        set => SetProperty(ref _lessonXp, value);
    }

    public ICommand LoadLessonCommand { get; }
    public ICommand MarkCompleteCommand { get; }
    public ICommand GoBackCommand { get; }

    public LessonViewModel(IContentService contentService, IProgressService progressService, IStreakService streakService)
    {
        _contentService = contentService;
        _progressService = progressService;
        _streakService = streakService;

        LoadLessonCommand = new AsyncRelayCommand(LoadLessonAsync);
        MarkCompleteCommand = new AsyncRelayCommand(MarkCompleteAsync);
        GoBackCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }

    private async Task LoadLessonAsync()
    {
        if (IsBusy || string.IsNullOrEmpty(LessonId)) return;
        IsBusy = true;

        try
        {
            var module = await _contentService.GetModuleAsync(PathId, ModuleId);
            var lessonInfo = module?.Lessons.FirstOrDefault(l => l.Id == LessonId);

            if (lessonInfo != null)
            {
                Title = lessonInfo.Title;
                LessonXp = lessonInfo.Xp;
                MarkdownContent = await _contentService.GetLessonContentAsync(PathId, ModuleId, lessonInfo.File);
            }

            IsCompleted = await _progressService.IsLessonCompletedAsync(PathId, ModuleId, LessonId);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading lesson: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task MarkCompleteAsync()
    {
        await _progressService.MarkLessonCompleteAsync(PathId, ModuleId, LessonId, LessonXp);
        await _streakService.RecordActivityAsync();
        IsCompleted = true;
        HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        // Delay to show XP popup animation before navigating back
        await Task.Delay(1800);
        await Shell.Current.GoToAsync("..");
    }
}
