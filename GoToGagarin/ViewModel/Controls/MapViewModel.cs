using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using GoToGagarin.Utilities;
using MapControlLib;
using MapControlLib.Models;
using System.Text.RegularExpressions;

namespace GoToGagarin.ViewModel.Controls;

public partial class MapViewModel : ObservableObject
{
    [ObservableProperty] private List<MapObject> _mapObjects;
    [ObservableProperty] private List<INavigateableModel> _allMapObjects;
    [ObservableProperty] private MapObject _selectObject;

    [ObservableProperty] private List<Floor> _floors;
    [ObservableProperty] private Floor _selectedFloor;

    [ObservableProperty] private List<Area> _areas;
    [ObservableProperty] private Area _terminalArea;

    [ObservableProperty] private List<Terminal>? _terminals;
    [ObservableProperty] private Terminal? _terminal;
    [ObservableProperty] private ControlVisibleModel _visible;

    [ObservableProperty] private List<InactivityModel> _inactivityModels;

    [ObservableProperty] private Map _map;
    [ObservableProperty] private double _scrollPosition;


    public const string RouteTypeCar = "На машине";
    public const string RouteTypeWalk = "Пешком";

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


    private readonly IMainApiClient _client;
    private readonly ImageLoadingHttpClient _imageClient;

    [ObservableProperty] private bool _buttonVisible;

    private readonly int _terminalId;
    [ObservableProperty] private bool _showNavigation;
    [ObservableProperty] private bool _isControlClose;
    [ObservableProperty] private double _zoomMin;
    [ObservableProperty] private double _zoomMax;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnabledMinus))]
    [NotifyPropertyChangedFor(nameof(IsEnabledPlus))]
    private double _currentZoom;

    public bool IsEnabledMinus => CurrentZoom > ZoomMin;
    public bool IsEnabledPlus => CurrentZoom < ZoomMax;

    public MapViewModel(
        ImageLoadingHttpClient imageClient, 
        IMainApiClient client, int terminalId)
    {
        _imageClient = imageClient;
        _client = client;
        _terminalId = terminalId;
        _visible = new ControlVisibleModel();
        ZoomMin = 2.0;
        ZoomMax = 10.0;
        RouteType = RouteTypeWalk;
    }

    [RelayCommand]
    private async Task Loaded(Map map)
    {
        InactivityModels = await _client.GetVideo();
        InactivityModels[0].media = await _imageClient.DownloadVideo(InactivityModels[0].media);

        Floors = await _client.GetFloors();
        foreach (var floor in Floors)
        {
            floor.ImagePath = await _imageClient.DownloadImage(floor.Image);
        }
        this.Map = map;
        MapObjects = [];
        Visible.SwitchControlVisible(ControlVisible.None);
        
        Areas = await _client.GetAreas();
        var mapObjectsModels = await _client.GetMapObjects();
        MapObjects = mapObjectsModels.ToMapObjects();
        Terminals = await _client.GetTerminals();
        Terminal = Terminals[_terminalId];
        TerminalArea = Areas.FirstOrDefault(f=>f.Id == Terminal.Area);
        AllMapObjects = [..MapObjects, Terminal];
        foreach (var imagesModel in MapObjects.SelectMany(mapObjectsModel => mapObjectsModel.Images))
        {
            imagesModel.Image = await _imageClient.DownloadImage(imagesModel.Image!);
        }



        SelectedFloor = Floors.FirstOrDefault()!;
    }

    [RelayCommand]
    private void SelectMapObject(MapObject f)
    {
        StopBuild();
        ScrollToStart();
        ScrollPosition = 0.0;
        SelectObject = f;
        ShowNavigation = false;
        Visible.SwitchControlVisible(ControlVisible.IsInfo);
    }

    [RelayCommand]
    public void ScrollToStart(){ ScrollToStartCommand.NotifyCanExecuteChanged(); }

    public async void BuildRouteByType()
    {
        StopBuild();

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

        BuildRoutes(RouteType);

        await Task.Delay(500); // Анимация
        ShowNavigation = true;
    }

    public void SetRouteVisibility(bool carVisible, bool walkVisible)
    {
        CarRouteIsVisible = carVisible;
        WalkRouteIsVisible = walkVisible;
    }


    public async void BuildRoutes(string type)
    {
        if (_routeTypeCodes.TryGetValue(type, out int code))
        {
            await BuildRoute(SelectObject, code);
        }
        else
        {
            await BuildRoute(SelectObject, 6);
        }
    }

    public async Task<List<NaviPoint>> Route(MapObject SelectedMapObject, int routeType)
    {
        if (Terminal?.Node is null || SelectedMapObject.Node is null) return null;
        var points = await _client.Navigate((int)Terminal.Node, (int)SelectedMapObject.Node, routeType);
        return points;
    }

    private async Task<List<NaviPoint>> CheckRoute(int type)
    {
        return await Route(SelectObject, type);
    }

    public async Task BuildRoute(MapObject SelectedMapObject, int routeType)
    {
        var points = await Route(SelectedMapObject, routeType);
        this.Map.Navigate(points);
    }

    public void StopBuild()
    {
        this.Map.CancelNavigation();
    }
}