using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace GoToGagarin.Behaviors
{
    public class MediaElementLoopBehavior : Behavior<MediaElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MediaEnded += OnMediaEnded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MediaEnded -= OnMediaEnded;
        }

        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Position = TimeSpan.Zero;
            AssociatedObject.Play();
        }
    }
}