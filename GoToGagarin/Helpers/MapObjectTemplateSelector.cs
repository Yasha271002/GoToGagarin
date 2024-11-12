using System.Windows;
using System.Windows.Controls;
using GoToGagarin.Model;

namespace GoToGagarin.Helpers;

public class MapObjectTemplateSelector:DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (container is not FrameworkElement control) return new DataTemplate();
        return item switch
        {
            Terminal => (DataTemplate)control.FindResource("TerminalTemplate"),
            MapObjectsModel => (DataTemplate)control.FindResource("MapObjectTemplate"),
            _ => base.SelectTemplate(item, container)
        };
    }
}