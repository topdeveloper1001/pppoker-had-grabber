using PPPokerCardCatcher.ViewModels;
using System.Windows.Forms;

namespace PPPokerCardCatcher.Views
{
    public class ErrorBox
    {
        public static void Show(string title, string notification, MessageBoxButtons buttons)
        {
            var notificationViewModelInfo = new NotificationViewModelInfo
            {
                Title = title,
                Notification = notification,
                Buttons = buttons
            };

            var notificationViewModel = new NotificationViewModel();
            notificationViewModel.Configure(notificationViewModelInfo);

            var notificationView = new ErrorView
            {
                DataContext = notificationViewModel,
                Owner = System.Windows.Application.Current.MainWindow
            };

            notificationViewModel.Closed += (s, a) => notificationView?.Close();

            notificationView.ShowDialog();
        }
    }
}