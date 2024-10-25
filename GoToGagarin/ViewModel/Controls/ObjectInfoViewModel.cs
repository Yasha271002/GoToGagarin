using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using GoToGagarin.Utilities;

namespace GoToGagarin.ViewModel.Controls;

public partial class ObjectInfoViewModel : ObservableObject
{
    private readonly IMainApiClient _client;
    private readonly ImageLoadingHttpClient _imageClient;

    [ObservableProperty] private List<MapObjectsModel>? _mapObjects;
    [ObservableProperty] private MapObjectsModel _selectObject;

    public ObjectInfoViewModel(IMainApiClient client, ImageLoadingHttpClient imageClient)
    {
        _client = client;
        _imageClient = imageClient;
    }

    [RelayCommand]
    private async Task Loaded()
    {
        //Floors = await _client.GetFloors();
        //foreach (var floor in Floors)
        //{
        //    floor.ImagePath = await _imageClient.DownloadImage(floor.Image);
        //}

        //this.Map = map;
        //Areas = await _client.GetAreas();

        MapObjects = await _client.GetMapObjects();
        SelectObject = MapObjects[0];
        //foreach (var mapObject in MapObjects.Where(mapObject => !string.IsNullOrEmpty(mapObject.Image)))
        //{
        //    mapObject.ImagePath = await _imageClient.DownloadImage(mapObject.Image);

        //    var area = Areas?.FirstOrDefault(x => x.Id == mapObject.Area);
        //    var floorName = Floors?.FirstOrDefault(c => c.Id == area?.Floor)?.Name;

        //    if (int.TryParse(floorName, out int parsedFloor))
        //    {
        //        mapObject.Floor = parsedFloor;
        //    }
        //    else
        //    {
        //        mapObject.Floor = 0;
        //    }

        //}

        //Terminal = await _client.GetTerminal(_terminalId);

        //TerminalArea = Areas?.FirstOrDefault(a => a.Id == Terminal.Area);
        //SelectedFloor = Floors?.FirstOrDefault();
    }
}