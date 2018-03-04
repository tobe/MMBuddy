using MahApps.Metro.Controls.Dialogs;
using MMBuddy.Dtos;
using MMBuddy.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace MMBuddy.ViewModel
{
    class MatchmakingViewModel : ObservableObject
    {
        private IDialogCoordinator _dialogCoordinator;
        private readonly Matchmaking _matchmaking;

        // For starting / stopping background async task
        CancellationTokenSource _cancellationTokenSource;

        // List of all champions (local JSON)
        private ObservableCollection<Champion> _champions = new ObservableCollection<Champion>();
        public ObservableCollection<Champion> Champions
        {
            get { return this._champions; }
            set
            {
                _champions = value;
                RaisePropertyChangedEvent(nameof(Champions));
            }
        }
        // Selected champion
        private Champion _selectedChampion;
        public Champion SelectedChampion
        {
            get { return this._selectedChampion; }
            set
            {
                this._selectedChampion = value;
                RaisePropertyChangedEvent(nameof(SelectedChampion));
            }
        }

        // Possible lanes
        private ObservableCollection<string> _lanes;
        public ObservableCollection<string> Lanes
        {
            get { return this._lanes; }
            set
            {
                this._lanes = value;
                RaisePropertyChangedEvent(nameof(Lanes));
            }
        }
        // Selected lane
        private string _selectedLane;
        public string SelectedLane
        {
            get { return this._selectedLane; }
            set
            {
                this._selectedLane = value;
                RaisePropertyChangedEvent(nameof(SelectedLane));
            }
        }

        public MatchmakingViewModel(IDialogCoordinator DialogCoordinator)
        {
            this._dialogCoordinator = DialogCoordinator;
            this._matchmaking = new Matchmaking();

            // Read all the champions into an observable collection
            this._champions = new ObservableCollection<Champion>(this._matchmaking.GetAllChampions());
            this._selectedChampion = this._champions[2];

            // Fill in the possibles lanes
            this._lanes = new ObservableCollection<string> { "Top", "Mid", "Bot", "Support", "Jungle" };
            this._selectedLane = this._lanes[1];
        }

        public async void MatchmakingStateChanged()
        {
            if(this._cancellationTokenSource == null)
            {
                this._cancellationTokenSource = new CancellationTokenSource();
                try
                {
                    await this._matchmaking.ProcessMatchmaking(
                        this._cancellationTokenSource.Token
                    );
                }catch (OperationCanceledException)
                {

                }finally
                {
                    this._cancellationTokenSource = null;
                }
            }else
            {
                this._cancellationTokenSource.Cancel();
                this._cancellationTokenSource = null;
            }
        }
    }
}
