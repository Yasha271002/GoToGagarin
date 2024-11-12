using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GoToGagarin.Model;
using GoToGagarin.ViewModel.Controls;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace GoToGagarin.ViewModel.Popup;

public partial class ContentSliderPopupViewModel : BasePopupViewModel
{
    [ObservableProperty] private MapViewModel _mapViewModel;
    [ObservableProperty] private List<ImagesModel>? _images;
    [ObservableProperty] private List<string>? _imageList;

    public ContentSliderPopupViewModel(MapViewModel mapViewModel, CloseNavigationService<ModalNavigationStore> closeNavigationService) : base(closeNavigationService)
    {
        ImageList = new List<string>();
        _mapViewModel = mapViewModel;
        Images = MapViewModel.SelectObject.Images;
        foreach (var image in Images)
        {
            ImageList.Add(image.Image);
        }
    }

}