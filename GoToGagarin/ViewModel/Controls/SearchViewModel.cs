﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using System.Collections.ObjectModel;

namespace GoToGagarin.ViewModel.Controls;

public partial class SearchViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapViewModel;
    [ObservableProperty] private string? _searchObjectName;
    [ObservableProperty] private ObservableCollection<MapObject>? _foundObjects;

    public SearchViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
        FoundObjects = [];
    }

    [RelayCommand]
    private void SelectObject(MapObject model)
    {
        MapViewModel.SelectObject = model;
        MapViewModel.Visible.SwitchControlVisible(ControlVisible.IsInfo);
        SearchObjectName = "";
    }

    [RelayCommand]
    private void Close()
    {
        MapViewModel.Visible.SwitchControlVisible(ControlVisible.None);
        SearchObjectName = "";
        MapViewModel.ButtonVisible = true;
    }

    [RelayCommand]
    private void TextChanged()
    {
        FoundObjects!.Clear();

        if (string.IsNullOrWhiteSpace(SearchObjectName)) return;

        var foundItems = MapViewModel.MapObjects
            .Where(f => 
                f.Title.Contains(SearchObjectName, StringComparison.OrdinalIgnoreCase));

        foreach (var mapObjectsModel in foundItems)
        {
            FoundObjects.Add(mapObjectsModel);
        }

        OnPropertyChanged(nameof(FoundObjects));
    }
}