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
        private int searchType;

        // Constructor
        public CommitteeListPage() {
            InitializeComponent();
        }

        // When page is navigated to, set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            HouseLoading.Visibility = Visibility.Visible;
            HouseListBox.Visibility = Visibility.Collapsed;
            SenateLoading.Visibility = Visibility.Visible;
            SenateListBox.Visibility = Visibility.Collapsed;

            Committee.allForChamber("House", loadHouseCommittees);
        }

        private void loadHouseCommittees(Collection<Committee> committees) {
            HouseLoading.Visibility = Visibility.Collapsed;
            HouseListBox.Visibility = Visibility.Visible;
            HouseListBox.DataContext = CommitteeListViewModel.fromCollection(committees);

            Committee.allForChamber("Senate", loadSenateCommittees);
        }

        private void loadSenateCommittees(Collection<Committee> committees) {
            SenateLoading.Visibility = Visibility.Collapsed;
            SenateListBox.Visibility = Visibility.Visible;
            SenateListBox.DataContext = CommitteeListViewModel.fromCollection(committees);
        }

        private void HouseListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            launchCommittee(HouseListBox);
        }

        private void SenateListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            launchCommittee(SenateListBox);
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
