using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using System.Collections.ObjectModel;
using MapControlLib;

namespace GoToGagarin.ViewModel.Controls;

public partial class SearchViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapViewModel;
    [ObservableProperty] private string? _searchObjectName;
    [ObservableProperty] private ObservableCollection<MapObjectsModel>? _foundObjects;

    public SearchViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
        FoundObjects = [];
    }

    [RelayCommand]
    private void SelectObject(MapObjectsModel model)
    {
        MapViewModel.SelectObject = model;
        MapViewModel.Visible.SwitchControlVisible(ControlVisible.IsInfo);
        SearchObjectName = "";
        OnPropertyChanged(nameof(MapViewModel.SelectObject));
    }

    [RelayCommand]
    private void Close()
    {
        MapViewModel.Visible.SwitchControlVisible(ControlVisible.None);
        SearchObjectName = "";
    }

    [RelayCommand]
    private void TextChanged()
    {
        FoundObjects!.Clear();

        if (string.IsNullOrWhiteSpace(SearchObjectName)) return;

        var foundItems = MapViewModel.MapObjects
            .Where(f => f.Title != null &&
                        f.Title.Contains(SearchObjectName, StringComparison.OrdinalIgnoreCase));

        foreach (var mapObjectsModel in foundItems)
        {
            FoundObjects.Add(mapObjectsModel);
        }

        OnPropertyChanged(nameof(FoundObjects));
    }
}