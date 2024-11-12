using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;

namespace GoToGagarin.ViewModel.Controls;

public partial class NavigationViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapViewModel;
    [ObservableProperty] private string? _routeType;

    public NavigationViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
        RouteType = string.Empty;
    }

    [RelayCommand]
    private void BuildRoute()
    {
        MapViewModel.StopBuild();
        MapViewModel.ShowNavigation = true;
        BuildRoute(RouteType!);
    }

    [RelayCommand]
    private void SwitchType(object parameter)
    {
        MapViewModel.StopBuild();
        RouteType = parameter.ToString();
        BuildRoute(RouteType!);
    }

    [RelayCommand]
    private async void Close()
    {
        MapViewModel.ShowNavigation = false;
        await Task.Delay(1500);
        MapViewModel.Visible.ControlVisible = ControlVisible.None;
    }

    private async void BuildRoute(string Type)
    {
        switch (RouteType)
        {
            case "На машине":
                await MapViewModel.BuildRoute(MapViewModel.SelectObject, 5);
                break;
            case "Пешком":
                await MapViewModel.BuildRoute(MapViewModel.SelectObject, 6);
                break;
            default:
                await MapViewModel.BuildRoute(MapViewModel.SelectObject, 6);
                break;
        }
    }
}