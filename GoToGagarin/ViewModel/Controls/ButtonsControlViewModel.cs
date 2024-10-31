using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;

namespace GoToGagarin.ViewModel.Controls;

public partial class ButtonsControlViewModel : ObservableObject
{
    [ObservableProperty] private bool _isEnabledPlus = true;
    [ObservableProperty] private bool _isEnabledMinus = true;

    [ObservableProperty] private MapViewModel _mapViewModel;

    public ButtonsControlViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
    }

    [RelayCommand]
    private void SearchButton()
    {
        MapViewModel.Visible.ControlVisible = ControlVisible.IsSearch;
        OnPropertyChanged(nameof(MapViewModel.Visible));
    }

    [RelayCommand]
    private void ZoomOut()
    {

    }

    [RelayCommand]
    private void ZoomIn()
    {

    }
}