using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using MapControlLib.Models;

namespace GoToGagarin.ViewModel.Controls;

public partial class NavigationViewModel : ObservableObject
{
    private const string RouteTypeCar = "На машине";
    private const string RouteTypeWalk = "Пешком";

    [ObservableProperty] private MapViewModel _mapViewModel;
    [ObservableProperty] private string? _routeType;
    [ObservableProperty] private bool _carRouteIsVisible;
    [ObservableProperty] private bool _walkRouteIsVisible;
    [ObservableProperty] private bool _selectedWalkRoute;
    [ObservableProperty] private bool _selectedCarRoute;

    private readonly Dictionary<string, int> _routeTypeCodes = new()
    {
        { RouteTypeCar, 5 },
        { RouteTypeWalk, 6 }
    };

    public NavigationViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
        RouteType = RouteTypeWalk;
    }

    [RelayCommand]
    private async void BuildRoute()
    {
        MapViewModel.StopBuild();

        var carPointTask = CheckRoute(5);
        var walkPointTask = CheckRoute(6);

        await Task.WhenAll(carPointTask, walkPointTask);

        var carRouteAvailable = carPointTask.Result?.Count > 0;
        var walkRouteAvailable = walkPointTask.Result?.Count > 0;

        switch (carRouteAvailable)
        {
            case false when walkRouteAvailable:
                RouteType = RouteTypeWalk;
                SetRouteVisibility(false, true);
                SelectedWalkRoute = true;
                break;
            case true when !walkRouteAvailable:
                RouteType = RouteTypeCar;
                SetRouteVisibility(true, false);
                SelectedCarRoute = true;
                break;
            default:
                RouteType ??= RouteTypeWalk;
                SetRouteVisibility(true, true);
                SelectedWalkRoute = true;
                break;
        }

        await Task.Delay(500);// Анимация

        BuildRoute(RouteType);

        await Task.Delay(500); // Анимация
        MapViewModel.ShowNavigation = true;
    }

    [RelayCommand]
    private async void SwitchType()
    {
        MapViewModel.StopBuild();
        await Task.Delay(500); // Анимация
        RouteType = SelectedCarRoute ? RouteTypeCar : RouteTypeWalk;
        BuildRoute(RouteType!);
    }

    [RelayCommand]
    private async void Close()
    {
        MapViewModel.ShowNavigation = false;
        SetRouteVisibility(true, true);
        SelectedCarRoute = false;
        SelectedWalkRoute = false;
        await Task.Delay(1500);
        MapViewModel.Visible.ControlVisible = ControlVisible.None;
    }

    private async Task<List<NaviPoint>> CheckRoute(int type)
    {
        return await MapViewModel.Route(MapViewModel.SelectObject, type);
    }

    private async void BuildRoute(string type)
    {
        if (_routeTypeCodes.TryGetValue(type, out int code))
        {
            await MapViewModel.BuildRoute(MapViewModel.SelectObject, code);
        }
        else
        {
            await MapViewModel.BuildRoute(MapViewModel.SelectObject, 6);
        }
    }

    private void SetRouteVisibility(bool carVisible, bool walkVisible)
    {
        CarRouteIsVisible = carVisible;
        WalkRouteIsVisible = walkVisible;
    }
}
