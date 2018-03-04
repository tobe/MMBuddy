using MahApps.Metro.Controls.Dialogs;
using MMBuddy.Dtos;
using MMBuddy.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace MMBuddy.ViewModel
{
    class MatchmakingViewModel : ObservableObject
    {
        private IDialogCoordinator _dialogCoordinator;
        private readonly Matchmaking _matchmaking;

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

        public MatchmakingViewModel(IDialogCoordinator DialogCoordinator)
        {
            this._dialogCoordinator = DialogCoordinator;
            this._matchmaking = new Matchmaking();

            // Read all the champions into an observable collection
            this._champions = new ObservableCollection<Champion>(this._matchmaking.GetAllChampions());
        }

        public async void MatchmakingStateChanged()
        {
            if(this._cancellationTokenSource == null)
            {
                this._cancellationTokenSource = new CancellationTokenSource();
                try
                {
                    await this._matchmaking.ProcessMatchmaking(this._cancellationTokenSource.Token);
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
