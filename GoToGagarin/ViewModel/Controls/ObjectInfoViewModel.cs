using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoToGagarin.Model;
using GoToGagarin.ViewModel.Popup;
using MvvmNavigationLib.Services;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using GoToGagarin.Helpers.Animation;

namespace GoToGagarin.ViewModel.Controls;

public partial class ObjectInfoViewModel : ObservableObject
{
    [ObservableProperty] private MapViewModel _mapVM;
    private ParameterNavigationService<ContentSliderPopupViewModel, MapViewModel> _parameterNavigationService;

    [ObservableProperty] private Grid _mainGrid;
    [ObservableProperty] private double _heightBorder;
    private readonly AnimationHelper _animationHelper;

    public ObjectInfoViewModel(MapViewModel mapViewModel,
        ParameterNavigationService<ContentSliderPopupViewModel, MapViewModel> parameterNavigationService)
    {
        _mapVM = mapViewModel;
        _parameterNavigationService = parameterNavigationService;
        _animationHelper = new AnimationHelper();
        HeightBorder = 298;
    }

    [RelayCommand]
    private void OpenContentSlider()
    {
        _parameterNavigationService.Navigate(MapVM);
    }

    [RelayCommand]
    private async void Close()
    {
        for (var i = HeightBorder; HeightBorder > 298; i -= 20)
        {
            HeightBorder = _animationHelper.Animation(HeightBorder);
            await Task.Delay(10);
        }

        await Task.Delay(100);
        MapVM.Visible.SwitchControlVisible(ControlVisible.None);
    }

    [RelayCommand]
    private void Navigate()
    {
        MapVM.Visible.SwitchControlVisible(ControlVisible.IsNavigate);
        HeightBorder = 298;
    }

    [RelayCommand]
    private async void IsDragging(MouseButtonEventArgs f)
    {
        if (f is not MouseButtonEventArgs e)
            return;


        while (e.ButtonState == MouseButtonState.Pressed)
        {
            var newPos = e.GetPosition(MainGrid);

            var height = 1920;
            var newHeight = (height) - newPos.Y * 1.8;

            newHeight = newHeight switch
            {
                > 1870 => 1856,
                < 278 => 298,
                _ => newHeight
            };

            HeightBorder = newHeight;

            await Task.Delay(10);
        }
    }
}