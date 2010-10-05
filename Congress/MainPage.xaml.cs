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
using Microsoft.Phone.Tasks;
using System.Windows.Controls.Primitives;

namespace Congress {

    public partial class MainPage : PhoneApplicationPage {
        public static int SEARCH_LOCATION = 0;
        public static int SEARCH_STATE = 1;
        public static int SEARCH_NAME = 2;
        public static int SEARCH_COMMITTEE = 3;

        public MainPage() {
            InitializeComponent();
        }

        private void LocationSearch(object sender, MouseEventArgs e) {
            NavigationService.Navigate(new Uri("/LegislatorListPage.xaml?searchType=" + SEARCH_LOCATION, UriKind.Relative));
        }
        
        private void StateSearch(object sender, MouseEventArgs e) {
            NavigationService.Navigate(new Uri("/StateListPage.xaml", UriKind.Relative));
        }

        private void NameSearch(object sender, MouseEventArgs e) {
            NavigationService.Navigate(new Uri("/LegislatorChamberPivotPage.xaml", UriKind.Relative));
        }

        private void CommitteeSearch(object sender, MouseEventArgs e) {
            NavigationService.Navigate(new Uri("/CommitteeListPage.xaml", UriKind.Relative));
        }


        private void sendFeedback(object sender, EventArgs e) {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = Congress.Strings.EmailSubject;
            task.To = Congress.Strings.EmailTo;
            task.Show();
        }

        private void showAbout(object sender, EventArgs e) {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}
