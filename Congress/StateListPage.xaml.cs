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

    public partial class StateListPage : PhoneApplicationPage {
        public StateListPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            DataContext = new StateNameList() {
                States = new ObservableCollection<StateName>() {
                    new StateName() {Name="Arizona"},
                    new StateName() {Name="Alabama"},
                    new StateName() {Name="Massachussetts"},
                }
            };
        }

        private void StateSelected(object sender, MouseEventArgs e) {
            StackPanel panel = sender as StackPanel;
            TextBlock stateText = panel.Children.ElementAt(0) as TextBlock;
            string stateName = stateText.Text;

            NavigationService.Navigate(new Uri("/LegislatorListPage.xaml?searchType=" + MainPage.SEARCH_STATE + "&stateName=" + stateName, UriKind.Relative));
        }

        public class StateName {
            public string Name {get; set;}
        }

        public class StateNameList {
            public ObservableCollection<StateName> States {get; set;}

            public StateNameList() {
                States = new ObservableCollection<StateName>();
            }
        }

    }
}