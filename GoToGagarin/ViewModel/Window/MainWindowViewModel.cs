using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GoToGagarin.Helpers;
using GoToGagarin.ViewModel.Controls;
using MainComponents.Popups;
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
    [ObservableProperty] private ButtonsControlViewModel _buttonsControlViewModel;

    private readonly ModalNavigationStore _modalNavigationStore;

    public ObservableObject? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
    public bool IsModalOpen => _modalNavigationStore.CurrentViewModel is not null;

    public MainWindowViewModel(InactivityHelper inactivityHelper,
        IMessenger messenger,
        ObjectInfoViewModel infoViewModel,
        NavigationViewModel navigationViewModel,
        SearchViewModel searchViewModel, 
        ButtonsControlViewModel buttonsControlViewModel,
        ModalNavigationStore modalNavigationStore)
    {
        messenger.RegisterAll(this);
        _inactivityHelper = inactivityHelper;
        _infoViewModel = infoViewModel;
        _navigationViewModel = navigationViewModel;
        _searchViewModel = searchViewModel;
        _buttonsControlViewModel = buttonsControlViewModel;
        _modalNavigationStore = modalNavigationStore;
        _inactivityHelper.OnInactivity += _inactivityHelper_OnInactivity;
    }

    public void _inactivityHelper_OnInactivity(int inactivityTime)
    {
        if (CurrentModalViewModel is BasePopupViewModel popup) popup.CloseContainerCommand.Execute(false);
    }

    private void Timer(object? sender, EventArgs eventArgs)
    {
        _sec++;
        if (_sec < 7) return;
        ExplorerHelper.RunExplorer();
        Application.Current.Shutdown();
    }

    [RelayCommand]
    private void Loaded()
    {
        ExplorerHelper.KillExplorer();
    }

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