using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using GoToGagarin.ViewModel.Popup;
using MvvmNavigationLib.Services;

namespace GoToGagarin.ViewModel.Controls;

public partial class ObjectInfoViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapVM;
    private ParameterNavigationService<ContentSliderPopupViewModel, MapViewModel> _parameterNavigationService;

    public ObjectInfoViewModel(MapViewModel mapViewModel,
        ParameterNavigationService<ContentSliderPopupViewModel, MapViewModel> parameterNavigationService)
    {
        _mapVM = mapViewModel;
        _parameterNavigationService = parameterNavigationService;
    }

    [RelayCommand]
    private void OpenContentSlider()
    {
        _parameterNavigationService.Navigate(MapVM);
    }

    [RelayCommand]
    private void Close()
    {
        MapVM.SwitchAnimation();
    }

    [RelayCommand]
    private void Navigate()
    {
        MapVM.Visible.SwitchControlVisible(ControlVisible.IsNavigate);
    }

    
}