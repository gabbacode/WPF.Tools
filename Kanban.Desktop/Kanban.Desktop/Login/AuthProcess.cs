using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;

namespace Kanban.Desktop.Login
{
    public class AuthProcess
    {
        public static async void Start(
            Task<LoginDialogData> getAutenticationData,
            Func<LoginDialogData, bool> autentification,
            Action autenticationSuccess,
            Action autenticationFail)
        {
            var loginData = await getAutenticationData.ConfigureAwait(true);
            if (loginData == null)
            {
                autenticationFail();
                return;
            }

            var autentificationResult = await Task.Run(() => autentification(loginData)).ConfigureAwait(true);
            if (autentificationResult)
            {
                // in ui thread
                autenticationSuccess();
                return;
            }
            else
            {
                // in ui thread
                autenticationFail();
                return;
            }
        }
    }
}
