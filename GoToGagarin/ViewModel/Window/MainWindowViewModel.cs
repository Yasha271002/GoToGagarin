using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Helpers;
using GoToGagarin.ViewModel.Controls;
using System.Windows;
using System.Windows.Threading;

namespace GoToGagarin.ViewModel.Window;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer = new();
    private int _sec;
    private readonly InactivityHelper _inactivityHelper;

    [ObservableProperty] private ObjectInfoViewModel _infoViewModel;

    public MainWindowViewModel(InactivityHelper inactivityHelper, ObjectInfoViewModel infoViewModel)
    {
        _inactivityHelper = inactivityHelper;
        _infoViewModel = infoViewModel;
        _inactivityHelper.OnInactivity += _inactivityHelper_OnInactivity;
    }

    public void _inactivityHelper_OnInactivity(int inactiviryTime)
    {
    }

    private void Timer(object? sender, EventArgs eventArgs)
    {
        _sec++;
        if (_sec < 7) return;
        ExplorerHelper.RunExplorer();
        Application.Current.Shutdown();
    }

    [RelayCommand]
    private void Loaded() => ExplorerHelper.KillExplorer();

    [RelayCommand]
    private void StopTimer()
    {
        _timer.Tick -= Timer;
        _timer.Stop();
        _sec = 0;
    }

    [RelayCommand]
    private void StartTimer()
    {
        _timer?.Stop();
        _sec = 0;
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer;
        _timer.Start();
    }
}