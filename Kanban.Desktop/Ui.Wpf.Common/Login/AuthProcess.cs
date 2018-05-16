using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;

namespace Ui.Wpf.Common
{
    public class AuthProcess
    {
        private const int LoginTryCount = 3;

        public static async void Start(
            Func<Task<LoginDialogData>> getAutenticationData,
            Func<LoginDialogData, bool> autentification,
            Action autenticationSuccess,
            Action autenticationFail)
        {
            var trysLeft = LoginTryCount;
            while  (trysLeft-- > 0)
            {
                var loginData = await getAutenticationData().ConfigureAwait(true);
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
                    continue;
                }
            }

            autenticationFail();
        }
    }
}
