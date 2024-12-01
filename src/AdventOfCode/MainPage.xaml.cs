using AdventOfCode.ViewModels;

namespace AdventOfCode;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        DataContext = ViewModel;
    }

    public MainViewModel ViewModel { get; } = new MainViewModel();
}
