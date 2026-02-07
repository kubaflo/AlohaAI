using System.Collections.ObjectModel;
using System.Windows.Input;
using AlohaAI.Services;

namespace AlohaAI.ViewModels;

public class SearchViewModel : BaseViewModel
{
    private readonly IContentService _contentService;

    private string _searchQuery = string.Empty;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            SetProperty(ref _searchQuery, value);
            SearchCommand.Execute(null);
        }
    }

    private bool _hasResults;
    public bool HasResults
    {
        get => _hasResults;
        set => SetProperty(ref _hasResults, value);
    }

    private bool _showEmpty;
    public bool ShowEmpty
    {
        get => _showEmpty;
        set => SetProperty(ref _showEmpty, value);
    }

    public ObservableCollection<SearchResultItem> Results { get; } = [];

    public ICommand SearchCommand { get; }
    public ICommand SelectResultCommand { get; }
    public ICommand ClearCommand { get; }

    public SearchViewModel(IContentService contentService)
    {
        _contentService = contentService;
        Title = "Search";

        SearchCommand = new AsyncRelayCommand(SearchAsync);
        SelectResultCommand = new AsyncRelayCommand<SearchResultItem>(async item =>
        {
            if (item != null)
                await Shell.Current.GoToAsync($"lesson?pathId={item.PathId}&moduleId={item.ModuleId}&lessonId={item.LessonId}");
        });
        ClearCommand = new RelayCommand(() =>
        {
            SearchQuery = string.Empty;
            Results.Clear();
            HasResults = false;
            ShowEmpty = false;
        });
    }

    private async Task SearchAsync()
    {
        var query = SearchQuery?.Trim();
        if (string.IsNullOrEmpty(query) || query.Length < 2)
        {
            Results.Clear();
            HasResults = false;
            ShowEmpty = false;
            return;
        }

        try
        {
            Results.Clear();
            var paths = await _contentService.GetPathsAsync();

            foreach (var path in paths)
            {
                var modules = await _contentService.GetModulesAsync(path.Id);
                foreach (var module in modules)
                {
                    foreach (var lesson in module.Lessons)
                    {
                        if (lesson.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            module.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                        {
                            Results.Add(new SearchResultItem
                            {
                                PathId = path.Id,
                                ModuleId = module.Id,
                                LessonId = lesson.Id,
                                LessonTitle = lesson.Title,
                                ModuleTitle = module.Title,
                                PathTitle = path.Title,
                                Xp = lesson.Xp,
                                PathColor = path.Color
                            });
                        }
                    }
                }
            }

            HasResults = Results.Count > 0;
            ShowEmpty = Results.Count == 0 && query.Length >= 2;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Search error: {ex.Message}");
        }
    }
}

public class SearchResultItem
{
    public string PathId { get; set; } = string.Empty;
    public string ModuleId { get; set; } = string.Empty;
    public string LessonId { get; set; } = string.Empty;
    public string LessonTitle { get; set; } = string.Empty;
    public string ModuleTitle { get; set; } = string.Empty;
    public string PathTitle { get; set; } = string.Empty;
    public int Xp { get; set; }
    public string PathColor { get; set; } = "#4A90D9";
    public string Subtitle => $"{PathTitle} → {ModuleTitle}";
}
