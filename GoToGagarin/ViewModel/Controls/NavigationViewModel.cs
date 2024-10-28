using CommunityToolkit.Mvvm.ComponentModel;

namespace GoToGagarin.ViewModel.Controls;

public partial class NavigationViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapViewModel;

    public NavigationViewModel(MapViewModel mapViewModel)
    {
        _mapViewModel = mapViewModel;
    }
}