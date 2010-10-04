using System;
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

            (HouseSpinner.FindName("LoadingText") as TextBlock).Text = "Loading committees...";
            HouseSpinner.Visibility = Visibility.Visible;
            HouseListBox.Visibility = Visibility.Collapsed;
            HouseMessage.Visibility = Visibility.Collapsed;

            (SenateSpinner.FindName("LoadingText") as TextBlock).Text = "Loading committees...";
            SenateSpinner.Visibility = Visibility.Visible;
            SenateListBox.Visibility = Visibility.Collapsed;
            SenateMessage.Visibility = Visibility.Collapsed;

            (JointSpinner.FindName("LoadingText") as TextBlock).Text = "Loading committees...";
            JointSpinner.Visibility = Visibility.Visible;
            JointListBox.Visibility = Visibility.Collapsed;
            JointMessage.Visibility = Visibility.Collapsed;

            Committee.allForChamber("House", loadHouseCommittees);
            Committee.allForChamber("Senate", loadSenateCommittees);
            Committee.allForChamber("Joint", loadJointCommittees);
        }

        private void loadHouseCommittees(Collection<Committee> committees) {
            HouseSpinner.Visibility = Visibility.Collapsed;

            if (committees != null) {
                HouseListBox.Visibility = Visibility.Visible;
                HouseListBox.DataContext = CommitteeListViewModel.fromCollection(committees);
            } else {
                (HouseMessage.FindName("Message") as TextBlock).Text = "There was a problem loading committee information.";
                HouseMessage.Visibility = Visibility.Visible;
            }
        }

        private void loadSenateCommittees(Collection<Committee> committees) {
            SenateSpinner.Visibility = Visibility.Collapsed;

            if (committees != null) {
                SenateListBox.Visibility = Visibility.Visible;
                SenateListBox.DataContext = CommitteeListViewModel.fromCollection(committees);
            } else {
                (SenateMessage.FindName("Message") as TextBlock).Text = "There was a problem loading committee information.";
                SenateMessage.Visibility = Visibility.Visible;
            }
        }

        private void loadJointCommittees(Collection<Committee> committees) {
            JointSpinner.Visibility = Visibility.Collapsed;

            if (committees != null) {
                JointListBox.Visibility = Visibility.Visible;
                JointListBox.DataContext = CommitteeListViewModel.fromCollection(committees);
            } else {
                (JointMessage.FindName("Message") as TextBlock).Text = "There was a problem loading committee information.";
                JointMessage.Visibility = Visibility.Visible;
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
