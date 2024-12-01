using System.Collections.ObjectModel;
using System.Diagnostics;
using AdventOfCode.Puzzles;
using Windows.Storage.Pickers;

namespace AdventOfCode.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly List<PuzzleSolutionInfo> _solutions;
    private readonly ApplicationDataContainer _localSettings;

    private readonly Stopwatch _stopwatch = new();

    // Selected values
    [ObservableProperty]
    private int? _selectedYear = -1;

    [ObservableProperty]
    private int? _selectedDay = -1;

    [ObservableProperty]
    private int? _selectedPart = -1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPuzzleSelected))]
    [NotifyPropertyChangedFor(nameof(HasTestData))]
    private PuzzleSolutionInfo? _selectedPuzzle;

    [ObservableProperty]
    private bool _hasInputFile = false;

    [ObservableProperty]
    private string _puzzleOutput;

    [ObservableProperty]
    private string _executionInfo;

    public MainViewModel()
    {
        _localSettings = ApplicationData.Current.LocalSettings;
        // Fetch puzzle solutions dynamically
        _solutions = new(SolutionLocator.GetPuzzleSolutions());
        LoadPuzzles();
        LoadSavedSelections();
    }

    // Collections for Years, Days, Parts, and Puzzles
    public ObservableCollection<int> AvailableYears { get; } = new();
    public ObservableCollection<int> AvailableDays { get; } = new();
    public ObservableCollection<int> AvailableParts { get; } = new();

    public bool IsPuzzleSelected => SelectedPuzzle != null;

    public bool HasTestData => SelectedPuzzle?.TestDataResourceName != null;

    private void LoadPuzzles()
    {
        // Clear previous values
        AvailableYears.Clear();
        AvailableDays.Clear();
        AvailableParts.Clear();

        // Add unique years to AvailableYears
        foreach (var year in _solutions.Select(p => p.Year).Distinct().OrderBy(y => y))
        {
            AvailableYears.Add(year);
        }
    }

    private void LoadSavedSelections()
    {
        var year = (int)(_localSettings.Values["SelectedYear"] ?? AvailableYears.FirstOrDefault());
        var day = (int)(_localSettings.Values["SelectedDay"] ?? AvailableDays.FirstOrDefault());
        var part = (int)(_localSettings.Values["SelectedPart"] ?? AvailableParts.FirstOrDefault());
        // Load saved values from local settings
        SelectedYear = year;
        SelectedDay = day;
        SelectedPart = part;
    }

    partial void OnSelectedYearChanged(int? value)
    {
        if (value.HasValue)
        {
            // Save selection to local settings
            _localSettings.Values["SelectedYear"] = value.Value;
        }

        // Update Days based on the selected year
        var days = _solutions
            .Where(p => p.Year == value)
            .Select(p => p.Day)
            .Distinct()
            .OrderBy(d => d);

        AvailableDays.Clear();
        foreach (var day in days)
        {
            AvailableDays.Add(day);
        }

        // Automatically select the first day
        if (AvailableDays.Any())
        {
            SelectedDay = AvailableDays.First();
        }

        UpdateSelectedPuzzle();
    }

    partial void OnSelectedDayChanged(int? value)
    {
        if (value.HasValue)
        {
            // Save selection to local settings
            _localSettings.Values["SelectedDay"] = value.Value;
        }

        // Update Parts based on the selected year and day
        var parts = _solutions
            .Where(p => p.Year == SelectedYear && p.Day == value)
            .Select(p => p.Part)
            .Distinct()
            .OrderBy(p => p);

        AvailableParts.Clear();
        foreach (var part in parts)
        {
            AvailableParts.Add(part);
        }

        // Automatically select the first part
        if (AvailableParts.Any())
        {
            SelectedPart = AvailableParts.First();
        }

        UpdateSelectedPuzzle();
    }

    partial void OnSelectedPartChanged(int? value)
    {
        if (value.HasValue)
        {
            // Save selection to local settings
            _localSettings.Values["SelectedPart"] = value.Value;
        }

        UpdateSelectedPuzzle();
    }

    private async void UpdateSelectedPuzzle()
    {
        SelectedPuzzle = _solutions.FirstOrDefault(p =>
            p.Year == SelectedYear && p.Day == SelectedDay && p.Part == SelectedPart);

        if (SelectedPuzzle != null)
        {
            HasInputFile = (await GetCachedInputFileAsync() is not null);
        }
        else
        {
            HasInputFile = false;
        }
    }

    [RelayCommand]
    public async Task RunWithTestDataAsync()
    {
        if (SelectedPuzzle?.TestDataResourceName is null)
        {
            throw new InvalidOperationException($"No test data available for the selected puzzle {SelectedPuzzle}.");
        }

        // Load the test data from the dynamically discovered resource name
        var testData = typeof(SolutionLocator).Assembly.GetManifestResourceStream(SelectedPuzzle.TestDataResourceName);

        if (testData is null)
        {
            throw new InvalidOperationException($"Resource {SelectedPuzzle.TestDataResourceName} not found.");
        }

        await RunWithInputAsync(SelectedPuzzle, testData);
    }

    private async Task RunWithInputAsync(PuzzleSolutionInfo puzzle, Stream input)
    {
        using var reader = new StreamReader(input);
        var puzzleInstance = (IPuzzleSolution?)Activator.CreateInstance(puzzle.PuzzleType);

        if (puzzleInstance is null)
        {
            throw new InvalidOperationException($"Puzzle instance could not be created for {SelectedPuzzle}.");
        }

        try
        {
            _stopwatch.Restart();
            ExecutionInfo = "Running...";

            PuzzleOutput = await puzzleInstance.SolveAsync(reader);
        }
        catch (Exception ex)
        {
            PuzzleOutput = $"Error: {ex.Message}";
        }
        finally
        {
            _stopwatch.Stop();
            ExecutionInfo = $"Execution time: {_stopwatch.ElapsedMilliseconds} ms";
        }
    }

    [RelayCommand(CanExecute = nameof(IsPuzzleSelected))]
    public async Task RunWithInputFileAsync()
    {
        if (SelectedPuzzle is null)
        {
            throw new InvalidOperationException("No puzzle selected.");
        }

        var inputFile = await GetCachedInputFileAsync();

        if (inputFile is null)
        {
            await PickInputFileAsync();

            // Try again after picking a file
            inputFile = await GetCachedInputFileAsync();

            if (inputFile is null)
            {
                throw new InvalidOperationException("No input file selected.");
            }
        }

        // Read the file contents
        var stream = await inputFile.OpenStreamForReadAsync();
        await RunWithInputAsync(SelectedPuzzle, stream);
    }

    [RelayCommand(CanExecute = nameof(IsPuzzleSelected))]
    public async Task PickInputFileAsync()
    {
        // Find the puzzle based on selected year/day/part
        if (SelectedPuzzle is null)
        {
            throw new InvalidOperationException("No puzzle selected.");
        }

        // Create a FileOpenPicker
        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.List,
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary
        };

        picker.FileTypeFilter.Add(".txt");
        picker.FileTypeFilter.Add("*");

        // Initialize picker for desktop applications
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        // Pick a file
        StorageFile inputFile = await picker.PickSingleFileAsync();

        if (inputFile == null)
        {
            PuzzleOutput = "No file was selected.";
            return;
        }

        // Cache the input file in ApplicationData
        string cachedFileName = GetCachedFileName(SelectedPuzzle.Year, SelectedPuzzle.Day, SelectedPuzzle.Part);
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        StorageFile cachedFile = await localFolder.CreateFileAsync(cachedFileName, CreationCollisionOption.ReplaceExisting);

        await inputFile.CopyAsync(localFolder, cachedFileName, NameCollisionOption.ReplaceExisting);

        PuzzleOutput = "File selected and cached.";

        HasInputFile = true;
    }

    private async Task<StorageFile?> GetCachedInputFileAsync()
    {
        string cachedFileName = GetCachedFileName(SelectedPuzzle.Year, SelectedPuzzle.Day, SelectedPuzzle.Part);
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        try
        {
            return await localFolder.GetFileAsync(cachedFileName);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private string GetCachedFileName(int year, int day, int part)
    {
        // Generate a unique filename for the problem
        return $"Puzzle_{year}_{day}_Part{part}.txt";
    }
}
