using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;

namespace GoToGagarin.ViewModel.Controls;

public partial class NavigationViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapViewModel;
    
    public NavigationViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
    }

    [RelayCommand]
    private void BuildRoute()
    {
        MapViewModel.ShowNavigation = true;
    }

    [RelayCommand]
    private async void Close()
    {
        MapViewModel.ShowNavigation = false;
        await Task.Delay(1500);
        MapViewModel.Visible.ControlVisible = ControlVisible.None;
    }
}