using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using GoToGagarin.Utilities;
using MapControlLib.Models;

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
    [ObservableProperty] private ControlVisibleModel _visible;


    private readonly IMainApiClient _client;
    private readonly ImageLoadingHttpClient _imageClient;

    private readonly int _terminalId;
    [ObservableProperty] private bool _showNavigation;
    [ObservableProperty] private bool _isControlClose;

    public MapViewModel(
        ImageLoadingHttpClient imageClient, 
        IMainApiClient client)
    {
        _imageClient = imageClient;
        _client = client;
        _visible = new ControlVisibleModel();

        Initialize();
    }

    private async void Initialize()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        MapObjects = [];
        Visible.SwitchControlVisible(ControlVisible.None);
        Floors = await _client.GetFloors();
        foreach (var floor in Floors)
        {
            floor.ImagePath = await _imageClient.DownloadImage(floor.Image);
        }
        Areas = await _client.GetAreas();
        MapObjects = await _client.GetMapObjects();

        Terminals = await _client.GetTerminals();
        TerminalArea = Areas.FirstOrDefault()!;

        foreach (var imagesModel in MapObjects.SelectMany(mapObjectsModel => mapObjectsModel.Images!))
        {
            imagesModel.Image = await _imageClient.DownloadImage(imagesModel.Image!);
        }

        SelectedFloor = Floors.FirstOrDefault()!;
    }

    [RelayCommand]
    private void SelectMapObject()
    {

    }
}