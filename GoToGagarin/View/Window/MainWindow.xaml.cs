using GoToGagarin.Helpers;
using System.Windows.Controls;

namespace GoToGagarin.View.Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PopupFrame_OnInitialized(object? sender, EventArgs e)
        {
            if (sender is not Frame frame) return;
            NavigationManager.PopupFrame = frame.NavigationService;
        }
    }
}