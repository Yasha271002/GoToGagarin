using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using GoToGagarin.Utilities;

namespace GoToGagarin.ViewModel.Controls;

public partial class MapViewModel : ObservableObject
{
    [ObservableProperty] private List<MapObjectsModel> _mapObjects;
    [ObservableProperty] private MapObjectsModel _selectObject;

    private readonly IMainApiClient _client;
    private readonly ImageLoadingHttpClient _imageClient;

    public MapViewModel(ImageLoadingHttpClient imageClient, IMainApiClient client)
    {
        _imageClient = imageClient;
        _client = client;
        Initialize();
    }

    private async void Initialize()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        MapObjects = [];
        MapObjects = await _client.GetMapObjects();

        foreach (var imagesModel in MapObjects.SelectMany(mapObjectsModel => mapObjectsModel.Images))
        {
            imagesModel.Image = await _imageClient.DownloadImage(imagesModel.Image);
        }

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

        SelectObject = MapObjects[0];
    }

    [RelayCommand]
    private void Loaded()
    {
    }
}