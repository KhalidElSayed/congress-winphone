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
using Congress.Models;
using Microsoft.Phone.Tasks;

namespace Congress {

    public partial class LegislatorPage : PhoneApplicationPage {
        private Legislator legislator;

        public LegislatorPage() {
            InitializeComponent();
        }

        // When page is navigated to, set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            string bioguideId = null;
            if (NavigationContext.QueryString.TryGetValue("bioguideId", out bioguideId)) {
                Legislator.find(bioguideId, new Legislator.LegislatorFoundEventHandler(displayLegislator));
            }
        }

        protected void displayLegislator(Legislator legislator) {
            this.legislator = legislator;
            DataContext = LegislatorViewModel.fromLegislator(legislator);
        }

        private void makeCall(object sender, MouseButtonEventArgs e) {
            PhoneCallTask call = new PhoneCallTask();
            call.PhoneNumber = legislator.phone;
            call.DisplayName = legislator.titledName();
            call.Show();
        }

        private void visitWebsite(object sender, MouseButtonEventArgs e) {
            WebBrowserTask web = new WebBrowserTask();
            web.URL = legislator.website;
            web.Show();
        }
    }
}
