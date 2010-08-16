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

namespace Congress {

    public partial class LegislatorListPage : PhoneApplicationPage {
        // Constructor
        public LegislatorListPage() {
            InitializeComponent();
        }

        // When page is navigated to, set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if (DataContext == null) {
                DataContext = new LegislatorListViewModel() {
                    Legislators = new ObservableCollection<LegislatorViewModel>() {
                        new LegislatorViewModel() {Name = "First Legislator"},
                        new LegislatorViewModel() {Name = "Second Legislator"},
                    }
                };
            }

            int index = -1;
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedIndex", out selectedIndex))
                index = int.Parse(selectedIndex);

            if (index == MainPage.SEARCH_LOCATION)
                ListTitle.Text = "by your location";
            else if (index == MainPage.SEARCH_LASTNAME)
                ListTitle.Text = "by last name";
            else if (index == MainPage.SEARCH_STATE)
                ListTitle.Text = "by state";
            else if (index == MainPage.SEARCH_ZIP)
                ListTitle.Text = "by zip code";
            else
                ListTitle.Text = "search results";
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;


            LegislatorViewModel legislator = (MainListBox.ItemsSource as ObservableCollection<LegislatorViewModel>)[MainListBox.SelectedIndex];

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/LegislatorPage.xaml?Name=" + legislator.Name, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }
    }
}
