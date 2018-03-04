using MahApps.Metro.Controls.Dialogs;
using MMBuddy.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMBuddy.View
{
    /// <summary>
    /// Interaction logic for MatchmakingControl.xaml
    /// </summary>
    public partial class MatchmakingControl : UserControl
    {
        private readonly MatchmakingViewModel _matchmakingViewModel =
            new MatchmakingViewModel(DialogCoordinator.Instance);

        public MatchmakingControl()
        {
            InitializeComponent();

            this.DataContext = this._matchmakingViewModel;
        }

        private void MatchmakingActive_Changed(object sender, RoutedEventArgs e)
        {
            this._matchmakingViewModel.MatchmakingStateChanged();
        }
    }
}
