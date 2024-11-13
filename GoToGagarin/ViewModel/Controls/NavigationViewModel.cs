using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using MapControlLib.Models;

namespace GoToGagarin.ViewModel.Controls;

public partial class NavigationViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapViewModel;

    public NavigationViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
    }

    

    [RelayCommand]
    private async void SwitchType()
    {
        await Task.Delay(500); // Анимация
        MapViewModel.RouteType = MapViewModel.SelectedCarRoute ? MapViewModel.RouteTypeCar : MapViewModel.RouteTypeWalk;
        MapViewModel.BuildRoutes(MapViewModel.RouteType!);
    }

    [RelayCommand]
    private async void Close()
    {
        MapViewModel.ShowNavigation = false;
        MapViewModel.SetRouteVisibility(true, true);
        MapViewModel.Visible.ControlVisible = ControlVisible.None;
    }
}
