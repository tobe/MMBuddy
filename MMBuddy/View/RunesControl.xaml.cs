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
    /// Interaction logic for RunesControl.xaml
    /// </summary>
    public partial class RunesControl : UserControl
    {
        /**
         * Initialize the ViewModel. If we did not have this, then this would be needed int he .xaml.cs file:
         * <UserControl.DataContext><ViewModel:RunesViewModel /></UserControl.DataContext>
         * Since the ViewModel ain't gonna initiate itself lol
         * */
        private readonly RunesViewModel _runesViewModel = new RunesViewModel(DialogCoordinator.Instance);

        public RunesControl()
        {
            InitializeComponent();

            this.DataContext = this._runesViewModel;
        }

        /// <summary>
        /// Runs once the usercontrol has been loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await this._runesViewModel.ShowStartupDialogAsync();
        }

        private void Testing_Click(object sender, RoutedEventArgs e)
        {
            this._runesViewModel.Testing();
        }
    }
}
