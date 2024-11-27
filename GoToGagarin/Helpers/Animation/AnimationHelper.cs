namespace GoToGagarin.Helpers.Animation;

public class AnimationHelper
{
    public double Animation(double height)
    {
        height -= 20;
        return height;
    }

    public double AnimationUp(double height)
    {
        height += 20;
        return height;
    }
}