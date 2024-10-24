using System.Windows.Controls;
using System.Windows.Input;
using Core;
using UserControl = System.Windows.Controls.UserControl;

namespace GoToGagarin.Helpers
{
    public static class CommonCommands
    {
        private static Page? GetPageByType(PageTypes pageType)
        {
            return pageType switch
            {
                PageTypes.None => null,
                _ => null
            };
        }

        private static Page? GetPageByContent(object content)
        {
            Page? page = content switch
            {
                _ => null
            };
            return page;
        }

        public static ICommand NavigateCommand { get; } = new RelayCommand(f =>
        {
            Page? page;
            if (f is PageTypes pageType)
            {
                page = GetPageByType(pageType);
            }
            else
            {
                page = GetPageByContent(f);
            }

            if (page == null) return;
            NavigationManager.MainFrame.Navigate(page);
        });

        private static UserControl? GetPopupByType(PopupTypes popupType)
        {
            return popupType switch
            {
                _ => null
            };
        }

        public static ICommand? GoBackCommand { get; } =
            new RelayCommand(f => { NavigationManager.MainFrame.GoBack(); });

        public static ICommand? GoBackPopupCommand { get; } = new RelayCommand(f =>
        {
            NavigationManager.PopupFrame.GoBack();
        });

        public static ICommand ClosePopupCommand { get; } =
            new RelayCommand(obj => { NavigationManager.ClosePopup(); });

        public static ICommand OpenPopupCommand { get; } = new RelayCommand(obj =>
        {
            UserControl? popup;
            if (obj is PopupTypes popupType)
            {
                popup = GetPopupByType(popupType);
            }
            else
            {
                popup = GetPopupByContent(obj);
            }

            if (popup is null)
                return;
            NavigationManager.PopupFrame?.Navigate(popup);
            NavigationManager.Instance.IsPopupOpen = true;
        });

        private static UserControl? GetPopupByContent(object content)
        {
            return content switch
            {
                _ => null
            };
        }
    }
}