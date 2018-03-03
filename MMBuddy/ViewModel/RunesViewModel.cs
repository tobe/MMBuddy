using MahApps.Metro.Controls.Dialogs;
using MMBuddy.Dtos;
using MMBuddy.Model;
using MMBuddy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMBuddy.ViewModel
{
    public class RunesViewModel : ObservableObject
    {
        private IDialogCoordinator _dialogCoordinator;
        private readonly Runes _runes;

        // Collection of available rune pages
        private ObservableCollection<RunePage> _runePages = new ObservableCollection<RunePage>();
        public ObservableCollection<RunePage> RunePages
        {
            get { return this._runePages; }
            set
            {
                _runePages = value;
                RaisePropertyChangedEvent(nameof(RunePages));
            }
        }
        private RunePage _selectedRunePage;
        public RunePage SelectedRunePage
        {
            get { return this._selectedRunePage; }
            set
            {
                _selectedRunePage = value;
                RaisePropertyChangedEvent(nameof(SelectedRunePage));
            }
        }

        /// <summary>
        /// Initializes the runes model and gets available runepages.
        /// </summary>
        /// <param name="DialogCoordinator"></param>
        public RunesViewModel(IDialogCoordinator DialogCoordinator)
        {
            this._dialogCoordinator = DialogCoordinator;
            this._runes = new Runes();

            // Load rune pages from the file (if any)
            if(this._runes.RunePagesExist())
                this._runePages = new ObservableCollection<RunePage>(this._runes.GetSavedRunePages());
        }

        /// <summary>
        /// Saves the current rune page into the Observable Collection. 
        /// </summary>
        public async void SaveCurrentRunePage()
        {
            var currentRunePage = await this._runes.GetCurrentRunePageAsync();
            this._runePages.Add(currentRunePage);

            // Make it selected while we're at it, why not.
            this.SelectedRunePage = currentRunePage;
        }

        /// <summary>
        /// Saves all rune pages
        /// </summary>
        public async void SaveAllRunePages()
        {
            if(this._runes.SaveAllRunePages(this._runePages))
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Success", "Runes have been successfully saved");
                return;
            }

            await this._dialogCoordinator.ShowMessageAsync(this, "Failure", "There has been a problem saving runes");
        }

        /// <summary>
        /// Shows the startup dialog and initializes the API Client.
        /// </summary>
        /// <returns></returns>
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

            // Initialize the API Client
            ApiClient.Initialize(this._runes.ServerInfo);

            // We have a good connection
            await controller.CloseAsync();
        }
    }
}
