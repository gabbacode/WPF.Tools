using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ui.Wpf.Common
{
    public class LoginDialog
    {
        public static Task<LoginDialogData> GetAutenticationDataTask(string initialUserName = "")
        {
            var mainview = Application.Current.MainWindow as MetroWindow;
            var loginTask = mainview.ShowLoginAsync(
                "Авторизация",
                "Пожалуйста, укажите имя и пароль",
                new LoginDialogSettings
                {
                    AnimateShow = true,
                    AnimateHide = true,
                    InitialUsername = initialUserName,
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
