using MahApps.Metro.Controls.Dialogs;
using MMBuddy.Dtos;
using MMBuddy.Model;
using System.Collections.ObjectModel;

namespace MMBuddy.ViewModel
{
    class MatchmakingViewModel : ObservableObject
    {
        private IDialogCoordinator _dialogCoordinator;
        private readonly Matchmaking _matchmaking;

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
    }
}
