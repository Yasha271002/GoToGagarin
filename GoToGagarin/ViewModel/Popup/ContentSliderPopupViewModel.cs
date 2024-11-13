using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    [ObservableProperty] private ImagesModel _image;
    [ObservableProperty] private int _index;

    public ContentSliderPopupViewModel(MapViewModel mapViewModel, CloseNavigationService<ModalNavigationStore> closeNavigationService) : base(closeNavigationService)
    {
        ImageList = new List<string>();
        _mapViewModel = mapViewModel;
        Images = MapViewModel.SelectObject.Images;
        Index = 0;
        Image = MapViewModel.SelectObject.Images[Index];
        foreach (var image in Images)
        {
            ImageList.Add(image.Image);
        }
    }

    [RelayCommand]
    private void NextTitle()
    {
        Index++;
        if (Index >= Images.Count)
        {
            Index = 0;
        }
        Image = Images[Index];
    }

    [RelayCommand]
    private void PreviewTitle()
    {
        Index--;
        if (Index < 0)
        {
            Index = Images.Count - 1;
        }
        Image = Images[Index];
    }
}