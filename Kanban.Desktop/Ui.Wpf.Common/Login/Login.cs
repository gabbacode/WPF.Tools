using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace Ui.Wpf.Common
{
    public class LoginDialog
    {
        public static Task<LoginDialogData> GetAutenticationDataTask()
        {
            var mainview = Application.Current.MainWindow as MetroWindow;
            var loginTask = mainview.ShowLoginAsync(
                "Авторизация",
                "Пожалуйста, укажите имя и пароль",
                new LoginDialogSettings
                {
                    AnimateShow = true,
                    AnimateHide = true,
                    AffirmativeButtonText = "Войти",
                    NegativeButtonText = "Закрыть",
                    NegativeButtonVisibility = Visibility.Visible,
                    UsernameWatermark = "Имя",
                    PasswordWatermark = "Пароль",
                    EnablePasswordPreview = true
                });

            return loginTask;
        }
    }
}
