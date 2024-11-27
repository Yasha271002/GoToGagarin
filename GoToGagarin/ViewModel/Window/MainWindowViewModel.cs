using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GoToGagarin.Helpers;
using GoToGagarin.Model;
using GoToGagarin.ViewModel.Controls;
using GoToGagarin.ViewModel.Popup;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using System.Windows;
using System.Windows.Threading;

namespace GoToGagarin.ViewModel.Window;

public partial class MainWindowViewModel : ObservableObject,
                        IRecipient<ModalViewModelChangedMessage>
{
    private readonly DispatcherTimer _timer = new();
    private readonly InactivityHelper _inactivityHelper;

    private int _sec;

    [ObservableProperty] private ObjectInfoViewModel _infoViewModel;
    [ObservableProperty] private NavigationViewModel _navigationViewModel;
    [ObservableProperty] private SearchViewModel _searchViewModel;
    [ObservableProperty] private MapViewModel _mapViewModel;

    private ParameterNavigationService<InactivityPopupViewModel, ObjectInfoViewModel> _parameterNavigationService;
    private readonly ModalNavigationStore _modalNavigationStore;

    public ObservableObject? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
    public bool IsModalOpen => _modalNavigationStore.CurrentViewModel is not null;

    public MainWindowViewModel(InactivityHelper inactivityHelper,
        ParameterNavigationService<InactivityPopupViewModel, ObjectInfoViewModel> parameterNavigationService,
        IMessenger messenger,
        ObjectInfoViewModel infoViewModel,
        NavigationViewModel navigationViewModel,
    SearchViewModel searchViewModel,
        ModalNavigationStore modalNavigationStore,
        MapViewModel mapViewModel)
    {
        messenger.RegisterAll(this);
        _inactivityHelper = inactivityHelper;
        _parameterNavigationService = parameterNavigationService;
        _infoViewModel = infoViewModel;
        _navigationViewModel = navigationViewModel;
        _searchViewModel = searchViewModel;
        _modalNavigationStore = modalNavigationStore;
        _mapViewModel = mapViewModel;
        _inactivityHelper.OnInactivity += _inactivityHelper_OnInactivity;
    }

    private void _inactivityHelper_OnInactivity(int inactivityTime)
    {
        _parameterNavigationService.Navigate(InfoViewModel);
        MapViewModel.StopBuild();
        MapViewModel.ShowNavigation = false;
        MapViewModel.Visible.SwitchControlVisible(ControlVisible.None);
    }

    private void Timer(object? sender, EventArgs eventArgs)
    {
        _sec++;
        if (_sec < 7) return;
        Application.Current.Shutdown();
    }

    [RelayCommand]
    private void SearchButton()
    {
        MapViewModel.StopBuild();
        MapViewModel.Visible.ControlVisible = ControlVisible.IsSearch;
        MapViewModel.ButtonVisible = false;
        OnPropertyChanged(nameof(MapViewModel.Visible));
    }

    [RelayCommand]
    private void Loaded()
    {
        ExplorerHelper.KillExplorer();
        MapViewModel.Visible.SwitchControlVisible(ControlVisible.None);
        MapViewModel.ButtonVisible = true;
        //await MapViewModel.LoadData();
    }

    [RelayCommand]
    private void Closing() => ExplorerHelper.RunExplorer();

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

    public void Receive(ModalViewModelChangedMessage message)
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        OnPropertyChanged(nameof(IsModalOpen));
    }
}