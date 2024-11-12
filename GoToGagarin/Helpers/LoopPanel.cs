using System.Windows;
using System.Windows.Controls;
using Size = System.Windows.Size;

namespace MainComponents.Panels;

public enum StartPos
{
    Center,
    Begin
    
}
public class LoopPanel:Panel
{
    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset), typeof(double), typeof(LoopPanel), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsArrange));

    public double Offset
    {
        get { return (double)GetValue(OffsetProperty); }
        set { SetValue(OffsetProperty, value); }
    }

    public static readonly DependencyProperty CurrentIndexProperty = DependencyProperty.Register(
        nameof(CurrentIndex), typeof(int), typeof(LoopPanel), new PropertyMetadata(default(int)));

    public int CurrentIndex
    {
        get { return (int)GetValue(CurrentIndexProperty); }
        set { SetValue(CurrentIndexProperty, value); }
    }

    public static readonly DependencyProperty CurrentCenterPointProperty = DependencyProperty.Register(
        nameof(CurrentCenterPoint), typeof(double), typeof(LoopPanel), new PropertyMetadata(default(double)));

    public double CurrentCenterPoint
    {
        get { return (double)GetValue(CurrentCenterPointProperty); }
        set { SetValue(CurrentCenterPointProperty, value); }
    }

    public static readonly DependencyProperty StartPosProperty = DependencyProperty.Register(
        nameof(StartPos), typeof(StartPos), typeof(LoopPanel), new FrameworkPropertyMetadata(default(StartPos), FrameworkPropertyMetadataOptions.AffectsArrange));

    public StartPos StartPos
    {
        get { return (StartPos)GetValue(StartPosProperty); }
        set { SetValue(StartPosProperty, value); }
    }

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation), typeof(Orientation), typeof(LoopPanel), new FrameworkPropertyMetadata(default(Orientation), FrameworkPropertyMetadataOptions.AffectsArrange));

    public Orientation Orientation
    {
        get { return (Orientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
    }
    public double _totalSize;
    private double _maxElementSize;
    private double _centerOffset;

    protected override Size MeasureOverride(Size availableSize)
    {
        _totalSize = 0;
        return Orientation == Orientation.Horizontal
            ? MeasureHorizontal(availableSize)
            : MeasureVertical(availableSize);
    }
    private Size MeasureHorizontal(Size availableSize)
    {
        foreach (UIElement child in Children)
        {
            child.Measure(availableSize);
            _totalSize += child.DesiredSize.Width;
            _maxElementSize = Math.Max(child.DesiredSize.Height, _maxElementSize);
        }

        _centerOffset = availableSize.Width / 2;
        availableSize = new Size(Math.Min(_totalSize, availableSize.Width), _maxElementSize);
        return availableSize;
    }
    private Size MeasureVertical(Size availableSize)
    {
        foreach (UIElement child in Children)
        {
            child.Measure(availableSize);
            _totalSize += child.DesiredSize.Height;
            _maxElementSize = Math.Max(child.DesiredSize.Width, _maxElementSize);
        }

        _centerOffset = availableSize.Height / 2;
        availableSize = new Size(_maxElementSize, Math.Min(availableSize.Height, _totalSize));
        return availableSize;
    }


    protected override Size ArrangeOverride(Size finalSize)
    {
        
        var halfSize = 0.0;
        if (CurrentIndex < Children.Count) halfSize = Orientation == Orientation.Horizontal ? EvalIndexAndCenter() : EvalIndexAndCenterVertical();

        var offset = EvalOffset(halfSize);

        var x = 0.0;
        foreach (UIElement child in Children)
        {
            if (Orientation == Orientation.Horizontal)
            {
                child.Arrange(offset > x + child.DesiredSize.Width
                    ? new Rect(_totalSize + x - offset, (_maxElementSize - child.DesiredSize.Height) / 2, child.DesiredSize.Width, child.DesiredSize.Height)
                    : new Rect(x - offset, (_maxElementSize - child.DesiredSize.Height) / 2, child.DesiredSize.Width, child.DesiredSize.Height));
                x += child.DesiredSize.Width;
            }
            else
            {
                child.Arrange(offset > x + child.DesiredSize.Height
                    ? new Rect((_maxElementSize - child.DesiredSize.Width) / 2, _totalSize + x - offset, child.DesiredSize.Width, child.DesiredSize.Height)
                    : new Rect((_maxElementSize - child.DesiredSize.Width) / 2, x - offset, child.DesiredSize.Width, child.DesiredSize.Height));
                x += child.DesiredSize.Height;
            }
            

        }
        return base.ArrangeOverride(finalSize);
    }
    private double EvalOffset(double halfSize)
    {
        double offset;
        if (StartPos == StartPos.Begin)
        {
            offset = Offset % _totalSize;
        }
        else
        {
            offset = (Offset - _centerOffset + halfSize) % _totalSize;
        }
        if (offset < 0)
            offset = _totalSize + offset;
        return offset;
    }
    private double EvalIndexAndCenter()
    {
        var off = Offset;

        var halfSize = Children[0].DesiredSize.Width / 2;
        var size = halfSize;
        for (var i = 1; i <= CurrentIndex; i++)
        {
            size += Children[i].DesiredSize.Width;
        }
        GetCenterPoint(size, halfSize);

        if (size < off % _totalSize)
        {
            CurrentIndex++;

            if (CurrentIndex == Children.Count)
            {
                Offset -= _totalSize;
                CurrentIndex = 0;
            }
        }

        if (!(size - Children[CurrentIndex].DesiredSize.Width > off % _totalSize)) return halfSize;

        CurrentIndex--;
        if (CurrentIndex >= 0) return halfSize;
        Offset += _totalSize;
        CurrentIndex = Children.Count - 1;

        return halfSize;
    }
    private double EvalIndexAndCenterVertical()
    {
        var off = Offset;

        var halfSize = Children[0].DesiredSize.Height / 2;
        var size = halfSize;
        for (var i = 1; i <= CurrentIndex; i++)
        {
            size += Children[i].DesiredSize.Height;
        }
        GetCenterPointVertical(size, halfSize);

        if (size < off % _totalSize)
        {
            CurrentIndex++;

            if (CurrentIndex == Children.Count)
            {
                Offset -= _totalSize;
                CurrentIndex = 0;
            }
        }

        if (!(size - Children[CurrentIndex].DesiredSize.Height > off % _totalSize)) return halfSize;

        CurrentIndex--;
        if (CurrentIndex >= 0) return halfSize;
        Offset += _totalSize;
        CurrentIndex = Children.Count - 1;

        return halfSize;
    }
    private double GetCenterPoint(double size, double halfSize) => StartPos == StartPos.Begin
        ? CurrentCenterPoint = size + halfSize - Children[CurrentIndex].DesiredSize.Width
        : CurrentCenterPoint = size - Children[CurrentIndex].DesiredSize.Width / 2;
    private double GetCenterPointVertical(double size, double halfSize) => StartPos == StartPos.Begin
        ? CurrentCenterPoint = size + halfSize - Children[CurrentIndex].DesiredSize.Height
        : CurrentCenterPoint = size - Children[CurrentIndex].DesiredSize.Height / 2;
    public void GetCenterByIndex(int index)
    {
        var halfSize = Children[0].DesiredSize.Width / 2;
        var size = halfSize;
        for (var i = 1; i <= index; i++)
        {
            size += Children[i].DesiredSize.Width;
        }
        GetCenterPoint(size, halfSize);
    }
    public void GetCenterByIndexVert(int index)
    {
        var halfSize = Children[0].DesiredSize.Height / 2;
        var size = halfSize;
        for (var i = 1; i <= index; i++)
        {
            size += Children[i].DesiredSize.Height;
        }
        GetCenterPointVertical(size, halfSize);
    }
}