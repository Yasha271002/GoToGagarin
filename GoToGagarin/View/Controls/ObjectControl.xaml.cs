using System.Windows;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GoToGagarin.ViewModel.Controls;

namespace GoToGagarin.View.Controls
{
    /// <summary>
    /// Логика взаимодействия для ObjectControl.xaml
    /// </summary>
    public partial class ObjectControl : UserControl
    {
        public ObjectControl()
        {
            InitializeComponent();
        }

        [RelayCommand]
        private async void IsDragging(MouseButtonEventArgs f)
        {
            if (f is not MouseButtonEventArgs e)
                return;

            MainBorder.ApplyAnimationClock(Border.HeightProperty, null);

            while (e.ButtonState == MouseButtonState.Pressed)
            {
                var newPos = e.GetPosition(MainCanvas);

                var height = 1920;
                var newHeight = height - newPos.Y;

                newHeight = newHeight switch
                {
                    > 1870 => 1856,
                    < 278 => 298,
                    _ => newHeight
                };

                ContentSlider.Visibility = newHeight > 1600 ? Visibility.Hidden : Visibility.Visible;
                MainBorder.Height = newHeight;

                await Task.Delay(10);
            }
        }
    }
}