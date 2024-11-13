using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using GoToGagarin.ViewModel.Controls;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace GoToGagarin.ViewModel.Popup;

public partial class InactivityPopupViewModel : BasePopupViewModel
{
    [ObservableProperty] private ObjectInfoViewModel _infoViewModel;
    [ObservableProperty] private string? _videoPath;

    public InactivityPopupViewModel(ObjectInfoViewModel infoViewModel,
        CloseNavigationService<ModalNavigationStore> closeNavigationService) : base(closeNavigationService)
    {
        _infoViewModel = infoViewModel;
        var videoFiles = Directory.GetFiles("Video");
        VideoPath = Path.GetFullPath(videoFiles[0]);

    }
}