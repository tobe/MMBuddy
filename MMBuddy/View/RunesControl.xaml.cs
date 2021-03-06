﻿using MahApps.Metro.Controls.Dialogs;
using MMBuddy.ViewModel;
using System.Windows;
using System.Windows.Controls;

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
        private static bool _controlLoaded = false;

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
            if(!_controlLoaded)
                await this._runesViewModel.ShowStartupDialogAsync();

            _controlLoaded = true;
        }

        private void SaveCurrentPage_Click(object sender, RoutedEventArgs e)
        {
            this._runesViewModel.SaveCurrentRunePage();
        }

        private void DeleteCurrentPage_Click(object sender, RoutedEventArgs e)
        {
            this._runesViewModel.DeleteCurrentPage();
        }

        private void SaveAllRunePages_Click(object sender, RoutedEventArgs e)
        {
            this._runesViewModel.SaveAllRunePages();
        }

        private void ApplySelectedPage_Click(object sender, RoutedEventArgs e)
        {
            this._runesViewModel.ApplySelectedPage();
        }
    }
}
