using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core;

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

        private ICommand? _isDraggingCommand;

        public ICommand IsDraggingCommand => _isDraggingCommand ??= new RelayCommand(async f =>
        {
            if (f is not MouseButtonEventArgs e)
                return;
            while (e.ButtonState == MouseButtonState.Pressed)
            {
                var newPos = e.GetPosition(MainCanvas);

                var height = 1920;
                var newHeight = height - newPos.Y;

                newHeight = newHeight switch
                {
                    > 1870 => 1856,
                    < 258 => 278,
                    _ => newHeight
                };

                MainBorder.Height = newHeight; 

                await Task.Delay(10);
            }
        });

    }
}
