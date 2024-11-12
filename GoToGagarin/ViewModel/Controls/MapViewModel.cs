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
    [ObservableProperty] private List<MapObjectsModel> _mapObjects;
    [ObservableProperty] private MapObjectsModel _selectObject;

    [ObservableProperty] private List<Floor> _floors;
    [ObservableProperty] private Floor _selectedFloor;

    [ObservableProperty] private List<Area> _areas;
    [ObservableProperty] private Area _terminalArea;

    [ObservableProperty] private List<Terminal>? _terminals;
    [ObservableProperty] private Terminal? _terminal;
    [ObservableProperty] private ControlVisibleModel _visible;

    [ObservableProperty] private Map _map;


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
        ZoomMin = 1.0;
        ZoomMax = 10.0;
    }

    [RelayCommand]
    private async Task Loaded(Map map)
    {


        Floors = await _client.GetFloors();
        foreach (var floor in Floors)
        {
            floor.ImagePath = await _imageClient.DownloadImage(floor.Image);
        }
        this.Map = map;
        MapObjects = [];
        Visible.SwitchControlVisible(ControlVisible.None);
        
        Areas = await _client.GetAreas();
        MapObjects = await _client.GetMapObjects();

        foreach (var mapObject in MapObjects)
        {
            mapObject.Description = FiltredText(mapObject.Description!);
        }

        Terminals = await _client.GetTerminals();
        Terminal = Terminals[_terminalId];
        TerminalArea = Areas.FirstOrDefault(f=>f.Id == Terminal.Area);

        foreach (var imagesModel in MapObjects.SelectMany(mapObjectsModel => mapObjectsModel.Images!))
        {
            imagesModel.Image = await _imageClient.DownloadImage(imagesModel.Image!);
        }

        SelectedFloor = Floors.FirstOrDefault()!;
    }

    [RelayCommand]
    private void SelectMapObject(MapObjectsModel f)
    {
        if (f is not MapObjectsModel mapObject)
            return;
        SelectObject = mapObject;
        Visible.SwitchControlVisible(ControlVisible.IsInfo);
    }

    private string FiltredText(string text)
    {
        return text == null ? string.Empty : Regex.Replace(text, @"[^а-яА-Я0-9\s.,:!?'\-]", "");
    }

    public async Task BuildRoute(MapObjectsModel SelectedMapObject, int routeType)
    {
        if (Terminal?.Node is null || SelectedMapObject?.Node is null) return;
        var points = await _client.Navigate((int)Terminal.Node, (int)SelectedMapObject.Node, routeType);
        this.Map.Navigate(points);
    }

    public void StopBuild()
    {
        this.Map.CancelNavigation();
    }
}