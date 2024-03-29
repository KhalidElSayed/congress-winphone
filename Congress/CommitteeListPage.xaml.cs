﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using Congress.Models;
using Congress.ViewModels;

namespace Congress {

    public partial class CommitteeListPage : PhoneApplicationPage {
        public CommitteeListPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if (!restoreContext("House_DataContext", HouseSpinner, HouseMessage, HouseListBox)) {
                (HouseSpinner.FindName("LoadingText") as TextBlock).Text = "Loading committees...";
                HouseSpinner.Visibility = Visibility.Visible;
                HouseListBox.Visibility = Visibility.Collapsed;
                HouseMessage.Visibility = Visibility.Collapsed;
                Committee.allForChamber("House", loadHouseCommittees);
            }

            if (!restoreContext("Senate_DataContext", SenateSpinner, SenateMessage, SenateListBox)) {
                (SenateSpinner.FindName("LoadingText") as TextBlock).Text = "Loading committees...";
                SenateSpinner.Visibility = Visibility.Visible;
                SenateListBox.Visibility = Visibility.Collapsed;
                SenateMessage.Visibility = Visibility.Collapsed;
                Committee.allForChamber("Senate", loadSenateCommittees);
            }

            if (!restoreContext("Joint_DataContext", JointSpinner, JointMessage, JointListBox)) {
                (JointSpinner.FindName("LoadingText") as TextBlock).Text = "Loading committees...";
                JointSpinner.Visibility = Visibility.Visible;
                JointListBox.Visibility = Visibility.Collapsed;
                JointMessage.Visibility = Visibility.Collapsed;
                Committee.allForChamber("Joint", loadJointCommittees);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            if (HouseListBox.DataContext != null)
                State["House_DataContext"] = HouseListBox.DataContext;

            if (SenateListBox.DataContext != null)
                State["Senate_DataContext"] = SenateListBox.DataContext;

            if (JointListBox.DataContext != null)
                State["Joint_DataContext"] = JointListBox.DataContext;
        }

        private bool restoreContext(string name, Spinner spinner, ListMessage message, ListBox listBox) {
            object context;
            if (State.TryGetValue(name, out context)) {
                spinner.Visibility = Visibility.Collapsed;
                message.Visibility = Visibility.Collapsed;
                listBox.DataContext = context;
                listBox.Visibility = Visibility.Visible;
                return true;
            } else
                return false;
        }

        private void loadHouseCommittees(Collection<Committee> committees) {
            loadCommittees(committees, HouseSpinner, HouseListBox, HouseMessage);
        }

        private void loadSenateCommittees(Collection<Committee> committees) {
            loadCommittees(committees, SenateSpinner, SenateListBox, SenateMessage);
        }

        private void loadJointCommittees(Collection<Committee> committees) {
            loadCommittees(committees, JointSpinner, JointListBox, JointMessage);
        }

        private void loadCommittees(Collection<Committee> committees, Spinner spinner, ListBox listBox, ListMessage message) {
            spinner.Visibility = Visibility.Collapsed;

            if (committees != null) {
                if (committees.Count > 0) {
                    listBox.Visibility = Visibility.Visible;
                    listBox.DataContext = CommitteeListViewModel.fromCollection(committees);
                } else {
                    (message.FindName("Message") as TextBlock).Text = "No committees found.";
                    message.Visibility = Visibility.Visible;
                }
            } else {
                (message.FindName("Message") as TextBlock).Text = "There was a problem loading committee information.";
                message.Visibility = Visibility.Visible;
            }
        }

        private void HouseListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            launchCommittee(HouseListBox);
        }

        private void SenateListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            launchCommittee(SenateListBox);
        }

        private void JointListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            launchCommittee(JointListBox);
        }

        private void launchCommittee(ListBox listBox) {
            if (listBox.SelectedIndex == -1)
                return;
            
            CommitteeViewModel model = (listBox.ItemsSource as ObservableCollection<CommitteeViewModel>)[listBox.SelectedIndex];
            NavigationService.Navigate(new Uri("/LegislatorListPage.xaml?searchType=" + MainPage.SEARCH_COMMITTEE + "&committeeId=" + model.committee.id + "&committeeName=" + model.Name, UriKind.Relative));

            listBox.SelectedIndex = -1;
        }
    }
}
