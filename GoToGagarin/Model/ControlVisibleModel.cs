using CommunityToolkit.Mvvm.ComponentModel;

namespace GoToGagarin.Model;


public enum ControlVisible
{
    IsSearch,
    IsNavigate,
    IsInfo,
    None
}

public partial class ControlVisibleModel : ObservableObject
{
    [ObservableProperty] private ControlVisible _controlVisible;

    public void SwitchControlVisible(ControlVisible controlVisible)
    {
        ControlVisible = controlVisible;
    }
}