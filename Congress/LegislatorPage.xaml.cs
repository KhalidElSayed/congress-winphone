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

namespace Congress {

    public partial class LegislatorPage : PhoneApplicationPage {
        public LegislatorPage() {
            InitializeComponent();
        }

        // When page is navigated to, set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            string bioguideId = null;
            if (NavigationContext.QueryString.TryGetValue("bioguideId", out bioguideId)) {
                
                bioguideId = "L000551"; // debug

                LegislatorBioguide.Text = bioguideId;
                LegislatorName.Text = "Loading...";

                Legislator.find(bioguideId, new Legislator.LegislatorFoundEventHandler(displayLegislator));
            }
        }

        protected void displayLegislator(Legislator legislator) {
            LegislatorName.Text = legislator.lastName;
        }
    }
}
