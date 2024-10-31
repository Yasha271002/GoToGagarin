using CommunityToolkit.Mvvm.ComponentModel;

namespace GoToGagarin.Model;

public enum AnimationState
{
    Start,
    Stop,
    None
}

public partial class AnimationModel : ObservableObject
{
    [ObservableProperty] private AnimationState _stateAnimation;

    public void SwitchState(AnimationState state)
    {
        StateAnimation = state;
    }
}