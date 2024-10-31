using CommunityToolkit.Mvvm.ComponentModel;
using GoToGagarin.ViewModel.Controls;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace GoToGagarin.ViewModel.Popup;

public partial class ContentSliderPopupViewModel : BasePopupViewModel
{
    [ObservableProperty] private MapViewModel _mapViewModel;

    public ContentSliderPopupViewModel(MapViewModel mapViewModel, CloseNavigationService<ModalNavigationStore> closeNavigationService) : base(closeNavigationService)
    {
        _mapViewModel = mapViewModel;
    }

}