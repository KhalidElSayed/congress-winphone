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

    public partial class LegislatorChamberPivotPage : PhoneApplicationPage {
        public LegislatorChamberPivotPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if (!restoreContext("House_DataContext", HouseSpinner, HouseMessage, HouseListBox)) {
                (HouseSpinner.FindName("LoadingText") as TextBlock).Text = "Loading legislators...";
                HouseSpinner.Visibility = Visibility.Visible;
                HouseListBox.Visibility = Visibility.Collapsed;
                HouseMessage.Visibility = Visibility.Collapsed;
                Legislator.findByChamber("House", loadHouseLegislators);
            }

            if (!restoreContext("Senate_DataContext", SenateSpinner, SenateMessage, SenateListBox)) {
                (SenateSpinner.FindName("LoadingText") as TextBlock).Text = "Loading legislators...";
                SenateSpinner.Visibility = Visibility.Visible;
                SenateListBox.Visibility = Visibility.Collapsed;
                SenateMessage.Visibility = Visibility.Collapsed;
                Legislator.findByChamber("Senate", loadSenateLegislators);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            if (HouseListBox.DataContext != null)
                State["House_DataContext"] = HouseListBox.DataContext;

            if (SenateListBox.DataContext != null)
                State["Senate_DataContext"] = SenateListBox.DataContext;
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

        private void loadLegislators(Spinner spinner, ListBox listBox, ListMessage message, Collection<Legislator> legislators) {
            spinner.Visibility = Visibility.Collapsed;

            if (legislators != null) {
                
                if (legislators.Count > 0) {
                    listBox.Visibility = Visibility.Visible;
                    listBox.DataContext = LegislatorListViewModel.fromCollection(legislators);
                } else {
                    (message.FindName("Message") as TextBlock).Text = "No legislators found.";
                    message.Visibility = Visibility.Visible;
                }

            } else {
                (message.FindName("Message") as TextBlock).Text = "There was a problem loading legislator information.";
                message.Visibility = Visibility.Visible;
            }
        }

        private void loadHouseLegislators(Collection<Legislator> legislators) {
            loadLegislators(HouseSpinner, HouseListBox, HouseMessage, legislators);
        }

        private void loadSenateLegislators(Collection<Legislator> legislators) {
            loadLegislators(SenateSpinner, SenateListBox, SenateMessage, legislators);
        }

        private void HouseListBox_SelectionChanged(object s, SelectionChangedEventArgs e) {
            LegislatorSelected(HouseListBox);
        }

        private void SenateListBox_SelectionChanged(object s, SelectionChangedEventArgs e) {
            LegislatorSelected(SenateListBox);
        }

        private void LegislatorSelected(ListBox listBox) {
            if (listBox.SelectedIndex == -1)
                return;

            LegislatorViewModel model = (listBox.ItemsSource as ObservableCollection<LegislatorViewModel>)[listBox.SelectedIndex];
            NavigationService.Navigate(new Uri("/LegislatorPivot.xaml?bioguideId=" + model.legislator.bioguideId + "&titledName=" + model.TitledName, UriKind.Relative));

            listBox.SelectedIndex = -1;
        }
    }
}