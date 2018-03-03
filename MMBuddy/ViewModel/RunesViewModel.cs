using MahApps.Metro.Controls.Dialogs;
using MMBuddy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMBuddy.ViewModel
{
    public class RunesViewModel : ObservableObject
    {
        private IDialogCoordinator _dialogCoordinator;
        private readonly Runes _runes;

        public RunesViewModel(IDialogCoordinator DialogCoordinator)
        {
            this._dialogCoordinator = DialogCoordinator;
            this._runes = new Runes();
        }

        public async Task ShowStartupDialogAsync()
        {
            var controller = await this._dialogCoordinator.ShowProgressAsync(this, "Please wait...", "Waiting for LeagueClientUx.exe...");
            controller.SetIndeterminate();

            while (true)
            {
                // Try to see if we have a good connection
                var serverInfo = this._runes.ServerInfo;
                if (serverInfo != null) break;

                // Reconnect every 1s
                this._runes.ReInitialize();
                await Task.Delay(1000);
            }

            // We have a good connection
            await controller.CloseAsync();
        }
    }
}
