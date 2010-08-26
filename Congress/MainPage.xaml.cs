﻿using System;
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
using System.Windows.Controls.Primitives;

namespace Congress {

    public partial class MainPage : PhoneApplicationPage {
        public static int SEARCH_LOCATION = 0;
        public static int SEARCH_ZIP = 1;
        public static int SEARCH_STATE = 2;
        public static int SEARCH_LASTNAME = 3;

        public MainPage() {
            InitializeComponent();
        }

        private void ZipSearch(object sender, MouseEventArgs e) {
            GetTextInput("Enter a zip code:", (zip) => {
                LaunchList(SEARCH_ZIP, "zip=" + zip);
            });
        }

        private void LastNameSearch(object sender, MouseEventArgs e) {
            GetTextInput("Enter a last name:", (lastName) => {
                LaunchList(SEARCH_LASTNAME, "lastName=" + lastName);
            });
        }

        private void StateSearch(object sender, MouseEventArgs e) {
            GetTextInput("Enter a state code:", (stateCode) => {
                LaunchList(SEARCH_STATE, "state=" + stateCode);
            });
        }

        private void GetTextInput(string ask, PopupTextResultHandler handler) {
            Popup popup = new Popup() {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Border border = new Border() {
                BorderBrush = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(5.0)
            };

            StackPanel main = new StackPanel() {
                Background = new SolidColorBrush(Colors.DarkGray)
            };

            TextBlock askText = new TextBlock() { Text = ask };
            TextBox input = new TextBox() { 
                Margin = new Thickness(5.0) 
            };

            Button ok = new Button() { 
                Content = "OK", 
                Margin = new Thickness(5.0) 
            };

            ok.Click += (s, e) => {
                popup.IsOpen = false;
                handler.Invoke(input.Text);
            };

            main.Children.Add(input);
            main.Children.Add(ok);
            border.Child = main;
            popup.Child = border;

            popup.IsOpen = true;
        }

        private delegate void PopupTextResultHandler(string result);

        private void LaunchList(int searchType, string queryString) {
            NavigationService.Navigate(new Uri("/LegislatorListPage.xaml?searchType=" + searchType + "&" + queryString, UriKind.Relative));
        }

        private void SearchName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

        }
    }
}
