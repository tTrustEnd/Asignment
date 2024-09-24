using AsignmentWinUI.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml.Controls;

namespace AsignmentWinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel;

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
