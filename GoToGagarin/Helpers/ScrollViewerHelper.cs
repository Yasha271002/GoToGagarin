using System.Windows.Controls;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Input;

namespace GoToGagarin.Helpers;

public class ScrollViewerHelper : Behavior<ScrollViewer>
{
    public static readonly DependencyProperty ScrollToStartCommandProperty =
        DependencyProperty.Register(
            nameof(ScrollToStartCommand),
            typeof(ICommand),
            typeof(ScrollViewerHelper),
            new PropertyMetadata(null, OnScrollToStartCommandChanged));

    public ICommand ScrollToStartCommand
    {
        get => (ICommand)GetValue(ScrollToStartCommandProperty);
        set => SetValue(ScrollToStartCommandProperty, value);
    }

    private static void OnScrollToStartCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewerHelper behavior)
        {
            if (e.OldValue is ICommand oldCommand)
            {
                oldCommand.CanExecuteChanged -= behavior.OnCanExecuteChanged;
            }

            if (e.NewValue is ICommand newCommand)
            {
                newCommand.CanExecuteChanged += behavior.OnCanExecuteChanged;
            }
        }
    }

    private void OnCanExecuteChanged(object sender, EventArgs e)
    {
        if (ScrollToStartCommand?.CanExecute(null) == true)
        {
            AssociatedObject.ScrollToHorizontalOffset(0);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (ScrollToStartCommand != null)
        {
            ScrollToStartCommand.CanExecuteChanged += OnCanExecuteChanged;
        }
    }

    protected override void OnDetaching()
    {
        if (ScrollToStartCommand != null)
        {
            ScrollToStartCommand.CanExecuteChanged -= OnCanExecuteChanged;
        }
        base.OnDetaching();
    }
}