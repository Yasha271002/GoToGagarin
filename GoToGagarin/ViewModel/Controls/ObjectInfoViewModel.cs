using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GoToGagarin.ViewModel.Controls;

public partial class ObjectInfoViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapVM;

    public ObjectInfoViewModel(MapViewModel mapViewModel)
    {
        _mapVM = mapViewModel;
    }

    [RelayCommand]
    private async Task Loaded()
    {
    }
}