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
        public static int SEARCH_ZIP = 1;
        public static int SEARCH_STATE = 2;
        public static int SEARCH_LASTNAME = 3;
        public static int SEARCH_COMMITTEE = 4;

        public MainPage() {
            InitializeComponent();
        }

        
        private void ZipSearch(object sender, MouseEventArgs e) {
            GetTextInput("Enter a zip code:", InputScopeNameValue.TelephoneNumber, (zip) => {
                LaunchList(SEARCH_ZIP, "zip=" + zip);
            });
        }

        private void LastNameSearch(object sender, MouseEventArgs e) {
            GetTextInput("Enter a last name:", InputScopeNameValue.PersonalSurname, (lastName) => {
                LaunchList(SEARCH_LASTNAME, "lastName=" + lastName);
            });
        }

        private void StateSearch(object sender, MouseEventArgs e) {
            GetTextInput("Enter a state code:", InputScopeNameValue.AddressCountryShortName, (stateCode) => {
                LaunchList(SEARCH_STATE, "state=" + stateCode);
            });
        }

        private void GetTextInput(string ask, InputScopeNameValue scopeValue, PopupTextResultHandler handler) {
            Popup popup = new Popup();

            // initialize the input scope
            InputScope scope = new InputScope();
            InputScopeName name = new InputScopeName() {
                NameValue = scopeValue
            };
            scope.Names.Add(name);

            Border border = new Border() {
                BorderBrush = new SolidColorBrush(new Color() {R = 96, G = 96, B = 96, A = 255}),
                BorderThickness = new Thickness(3.0)
            };

            StackPanel main = new StackPanel() {
                Background = new SolidColorBrush(new Color() {R = 24, G = 40, B = 75, A = 255})
            };

            TextBlock askText = new TextBlock() { 
                Text = ask,
                Margin = new Thickness(17, 10, 17, 5),
                FontSize = 28.0,
                Foreground = new SolidColorBrush(Colors.White)
            };

            TextBox input = new TextBox() { 
                Margin = new Thickness(10, 5, 0, 5),
                InputScope = scope,
                Foreground = new SolidColorBrush(Colors.Black),
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };

            StackPanel buttons = new StackPanel() {
                Margin = new Thickness(0, 15, 0, 0),
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                Background = new SolidColorBrush(new Color() {R = 48, G = 48, B = 48, A = 255}),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Button ok = new Button() { 
                Content = "OK", 
                Margin = new Thickness(10, 0, 3, 0),
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.White)
            };

            Button cancel = new Button() {
                Content = "Cancel",
                Margin = new Thickness(3, 0, 10, 0),
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.White)
            };

            ok.Click += (s, e) => {
                popup.IsOpen = false;
                
                handler.Invoke(input.Text);
            };

            cancel.Click += (s, e) => {
                popup.IsOpen = false;
            };

            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            
            main.Children.Add(askText);
            main.Children.Add(input);
            main.Children.Add(buttons);
            border.Child = main;
            popup.Child = border;

            popup.HorizontalOffset = 100;
            popup.VerticalOffset = 100;

            popup.IsOpen = true;
        }

        private delegate void PopupTextResultHandler(string result);

        private void LaunchList(int searchType, string queryString) {
            NavigationService.Navigate(new Uri("/LegislatorListPage.xaml?searchType=" + searchType + "&" + queryString, UriKind.Relative));
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
