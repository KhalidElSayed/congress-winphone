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

    public partial class LegislatorListPage : PhoneApplicationPage {
        private int searchType;

        // Constructor
        public LegislatorListPage() {
            InitializeComponent();
        }

        // When page is navigated to, set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            string searchTypeString = "";
            if (NavigationContext.QueryString.TryGetValue("searchType", out searchTypeString))
                searchType = int.Parse(searchTypeString);

            string state;
            NavigationContext.QueryString.TryGetValue("state", out state);

            string lastName;
            NavigationContext.QueryString.TryGetValue("lastName", out lastName);

            string zip;
            NavigationContext.QueryString.TryGetValue("zip", out zip);

            string committeeId, committeeName;
            NavigationContext.QueryString.TryGetValue("committeeId", out committeeId);
            NavigationContext.QueryString.TryGetValue("committeeName", out committeeName);

            MainListBox.Visibility = Visibility.Collapsed;
            ListMessage.Visibility = Visibility.Collapsed;
            (Spinner.FindName("LoadingText") as TextBlock).Text = "Finding legislators...";
            Spinner.Visibility = Visibility.Visible;

            if (searchType == MainPage.SEARCH_LOCATION) {
                MainTitle.Text = "for your location";
            }

            else if (searchType == MainPage.SEARCH_LASTNAME) {
                MainTitle.Text = "named \"" + lastName + "\"";
                Legislator.findByLastName(lastName, loadLegislators);
            }

            else if (searchType == MainPage.SEARCH_STATE) {
                MainTitle.Text = "for " + Legislator.stateCodeToName(state);
                Legislator.findByState(state, loadLegislators);
            }

            else if (searchType == MainPage.SEARCH_ZIP) {
                MainTitle.Text = "for " + zip;
                Legislator.findByZip(zip, loadLegislators);
            } 

            else if (searchType == MainPage.SEARCH_COMMITTEE) {
                MainTitle.FontSize = 24;
                MainTitle.Text = committeeName;
                Committee.find(committeeId, (committee) => {
                    if (committee != null)
                        loadLegislators(committee.members);
                    else
                        loadLegislators(null);
                });
            }
        }

        private void loadLegislators(Collection<Legislator> legislators) {
            Spinner.Visibility = Visibility.Collapsed;

            if (legislators != null) {
                MainListBox.Visibility = Visibility.Visible;

                if (DataContext == null)
                    DataContext = LegislatorListViewModel.fromCollection(legislators);
            } else {
                (ListMessage.FindName("Message") as TextBlock).Text = "There was a problem loading legislator information.";
                ListMessage.Visibility = Visibility.Visible;
            }
        }

        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (MainListBox.SelectedIndex == -1)
                return;

            LegislatorViewModel model = (MainListBox.ItemsSource as ObservableCollection<LegislatorViewModel>)[MainListBox.SelectedIndex];

            NavigationService.Navigate(new Uri("/LegislatorPivot.xaml?bioguideId=" + model.legislator.bioguideId + "&titledName=" + model.TitledName, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }
    }
}
