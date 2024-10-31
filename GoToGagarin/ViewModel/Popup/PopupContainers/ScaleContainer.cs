using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace GoToGagarin.ViewModel.Popup.PopupContainers;

public class ScaleContainer : BasePopupContainer
{

    protected override void ContainerLoaded(object sender, RoutedEventArgs e)
    {
        RenderTransform = new ScaleTransform(0, 0, ActualWidth / 2, ActualHeight / 2);
        var xAnimation = new DoubleAnimation(1, Duration)
        {
            EasingFunction = EasingFunction
        };
        var yAnimation = new DoubleAnimation(1, Duration)
        {
            EasingFunction = EasingFunction
        };
        StartAnimation(xAnimation, yAnimation);
    }

    public override void Close()
    {
        var xAnimation = new DoubleAnimation(0, Duration)
        {
            EasingFunction = EasingFunction
        };
        var yAnimation = new DoubleAnimation(0, Duration)
        {
            EasingFunction = EasingFunction
        };
        yAnimation.Completed += (o, args) => RaiseEvent(new RoutedEventArgs(ClosedEvent));
        StartAnimation(xAnimation, yAnimation);
    }

    protected override void StartAnimation(params Timeline[] animations)
    {
        var sb = new Storyboard();
        sb.Children.Add(animations[0]);
        sb.Children.Add(animations[1]);
        Storyboard.SetTarget(animations[0], this);
        Storyboard.SetTarget(animations[1], this);
        Storyboard.SetTargetProperty(animations[0], new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
        Storyboard.SetTargetProperty(animations[1], new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
        sb.Begin();
    }
}