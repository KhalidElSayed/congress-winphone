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

            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("searchType", out selectedIndex))
                searchType = int.Parse(selectedIndex);

            string state;
            NavigationContext.QueryString.TryGetValue("state", out state);

            string lastName;
            NavigationContext.QueryString.TryGetValue("lastName", out lastName);

            string zip;
            NavigationContext.QueryString.TryGetValue("zip", out zip);

            // debug, lock to VA legislators for now
            searchType = MainPage.SEARCH_STATE;
            state = "VA";

            // turn on Loading... message
            Loading.Visibility = Visibility.Visible;
            MainListBox.Visibility = Visibility.Collapsed;

            if (searchType == MainPage.SEARCH_LOCATION) {
                ListTitle.Text = "for your location";
            }
            else if (searchType == MainPage.SEARCH_LASTNAME) {
                ListTitle.Text = "named \"" + lastName + "\"";
            }
            else if (searchType == MainPage.SEARCH_STATE) {
                ListTitle.Text = "for " + state;
                Legislator.findByState(state, loadLegislators);
            }
            else if (searchType == MainPage.SEARCH_ZIP) {
                ListTitle.Text = "for " + zip;
            }
        }

        private void loadLegislators(Collection<Legislator> legislators) {
            Loading.Visibility = Visibility.Collapsed;
            MainListBox.Visibility = Visibility.Visible;
            if (DataContext == null) {
                DataContext = LegislatorListViewModel.fromCollection(legislators);
            }
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (MainListBox.SelectedIndex == -1)
                return;

            LegislatorViewModel model = (MainListBox.ItemsSource as ObservableCollection<LegislatorViewModel>)[MainListBox.SelectedIndex];

            NavigationService.Navigate(new Uri("/LegislatorPage.xaml?bioguideId=" + model.legislator.bioguideId, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }
    }
}
