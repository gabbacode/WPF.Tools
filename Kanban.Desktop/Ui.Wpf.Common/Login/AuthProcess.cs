using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;

namespace Ui.Wpf.Common
{
    public class AuthProcess
    {
        private const int LoginTryCount = 3;

        public static async void Start(
            Func<Task<LoginDialogData>> getAuthenticationData,
            Func<LoginDialogData, Task<bool>> authentication,
            Action authenticationSuccess,
            Action authenticationFail)
        {
            var trysLeft = LoginTryCount;
            while  (trysLeft-- > 0)
            {
                var loginData = await getAuthenticationData().ConfigureAwait(true);
                if (loginData == null)
                {
                    authenticationFail();
                    return;
                }

                var authentcationResult = await authentication(loginData).ConfigureAwait(true);
                if (authentcationResult)
                {
                    // in ui thread
                    authenticationSuccess();
                    return;
                }
                else
                {
                    // in ui thread
                    continue;
                }
            }

            authenticationFail();
        }
    }
}
