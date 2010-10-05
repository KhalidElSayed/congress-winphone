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
using System.Device.Location;

namespace Congress {

    public partial class LegislatorListPage : PhoneApplicationPage {
        private int searchType;
        GeoCoordinateWatcher watcher;

        public LegislatorListPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            string searchTypeString = "";
            if (NavigationContext.QueryString.TryGetValue("searchType", out searchTypeString))
                searchType = int.Parse(searchTypeString);

            string stateName;
            NavigationContext.QueryString.TryGetValue("stateName", out stateName);

            string committeeId, committeeName;
            NavigationContext.QueryString.TryGetValue("committeeId", out committeeId);
            NavigationContext.QueryString.TryGetValue("committeeName", out committeeName);

            MainListBox.Visibility = Visibility.Collapsed;
            ListMessage.Visibility = Visibility.Collapsed;
            
            if (searchType == MainPage.SEARCH_LOCATION) {
                MainTitle.Text = "for your location";
                setSpinner("Finding your location. This can take a while...");
                startFetchingLocation();
            }

            else if (searchType == MainPage.SEARCH_STATE) {
                MainTitle.Text = "for " + stateName;
                setSpinner("Finding legislators for " + stateName + "...");
                Legislator.findByState(Legislator.stateNameToCode(stateName), loadLegislators);
            }

            else if (searchType == MainPage.SEARCH_COMMITTEE) {
                MainTitle.FontSize = 24;
                MainTitle.Text = committeeName;
                setSpinner("Finding members of the " + committeeName + "...");
                Committee.find(committeeId, (committee) => {
                    if (committee != null)
                        loadLegislators(committee.members);
                    else
                        loadLegislators(null);
                });
            }
        }

        private void startFetchingLocation() {
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 20;
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(onLocationStatusChanged);
            watcher.Start();
        }

        private void onLocationStatusChanged(object sender, GeoPositionStatusChangedEventArgs e) {
            switch (e.Status) {
                case GeoPositionStatus.Disabled:
                    Spinner.Visibility = Visibility.Collapsed;

                    if (watcher.Permission == GeoPositionPermission.Denied)
                        setListMessage("To find legislators for your location, you need to change your phone's settings to allow apps to use your location.");
                    else
                        setListMessage("This device does not support finding your location.");
                    break;

                case GeoPositionStatus.Initializing:
                    break;

                case GeoPositionStatus.NoData:
                    Spinner.Visibility = Visibility.Collapsed;
                    setListMessage("There was a problem finding your location. Please try again later, or from another location.");
                    break;

                case GeoPositionStatus.Ready:
                    setSpinner("Finding legislators for your location...");
                    
                    GeoCoordinate coordinate = watcher.Position.Location;
                    string latitude = coordinate.Latitude.ToString("0.00000");
                    string longitude = coordinate.Longitude.ToString("0.00000");

                    //debug: bucharest, romania
                    //latitude = "44.4340";
                    //longitude = "26.0819";

                    //debug: houston, texas
                    //latitude = "29.7744";
                    //longitude = "-95.4231";

                    // turn off location service
                    watcher.Stop();

                    Legislator.findByLocation(latitude, longitude, loadLegislators);

                    break;
            }
        }

        private void loadLegislators(Collection<Legislator> legislators) {
            Spinner.Visibility = Visibility.Collapsed;

            if (legislators != null) {
                
                if (legislators.Count > 0) {
                    MainListBox.Visibility = Visibility.Visible;
                    DataContext = LegislatorListViewModel.fromCollection(legislators);
                } else {
                    if (searchType == MainPage.SEARCH_LOCATION)
                        setListMessage("No legislators found for your location.");
                    else
                        setListMessage("No legislators found.");
                }

            } else {
                // location from not within a congressional district will not return a JSON object, which returns null
                // better to assume that for location searches this is the issue
                if (searchType == MainPage.SEARCH_LOCATION)
                    setListMessage("No legislators found for your location.");
                else
                    setListMessage("There was a problem loading legislator information.");
            }
        }

        private void setListMessage(string message) {
            (ListMessage.FindName("Message") as TextBlock).Text = message;
            ListMessage.Visibility = Visibility.Visible;
        }

        private void setSpinner(string message) {
            (Spinner.FindName("LoadingText") as TextBlock).Text = message;
            Spinner.Visibility = Visibility.Visible;
        }

        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (MainListBox.SelectedIndex == -1)
                return;

            LegislatorViewModel model = (MainListBox.ItemsSource as ObservableCollection<LegislatorViewModel>)[MainListBox.SelectedIndex];
            NavigationService.Navigate(new Uri("/LegislatorPivot.xaml?bioguideId=" + model.legislator.bioguideId + "&titledName=" + model.TitledName, UriKind.Relative));

            MainListBox.SelectedIndex = -1;
        }
    }
}
