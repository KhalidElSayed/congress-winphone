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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Congress.Models;
using Congress.ViewModels;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace Congress {
    public partial class LegislatorPivot : PhoneApplicationPage {
        private LegislatorViewModel view;

        private PivotItem TweetsPivot;
        private ListBox TweetsList;

        private PivotItem VideosPivot;
        private ListBox VideosList;

        public LegislatorPivot() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            string bioguideId = null;
            if (NavigationContext.QueryString.TryGetValue("bioguideId", out bioguideId)) {
                Legislator.find(bioguideId, new Legislator.LegislatorFoundEventHandler(displayLegislator));
            }
        }

        protected void displayLegislator(Legislator legislator) {
            this.view = LegislatorViewModel.fromLegislator(legislator);
            DataContext = view;
            
            // trigger news fetching
            NewsItem.search(view.NewsKeyword, displayNews);

            // if twitter_id then add pivot and trigger tweet fetching
            if (view.legislator.twitterId != null && view.legislator.twitterId.Length > 0) {
                Tweet.search(legislator.twitterId, displayTweets);
            }

            // if youtube_url then add pivot and trigger video fetching
            if (view.legislator.youtubeUrl != null && view.legislator.youtubeUrl.Length > 0) {
                Video.getVideos(youtubeUsername(legislator.youtubeUrl), displayVideos);
            }
        }

        protected void displayNews(Collection<NewsItem> items) {
            NewsPivot.DataContext = NewsItemListViewModel.fromCollection(items);
        }

        protected void displayTweets(Collection<Tweet> tweets) {
            TweetsPivot = new PivotItem() { Name = "TweetsPivot", Header = "tweets" };

            TweetsList = new ListBox() {
                Name = "TweetsList",
                BorderThickness = new Thickness(0.0),
                BorderBrush = new SolidColorBrush(Colors.Transparent)
            };

            string template = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" +
                    "<TextBlock Text=\"{Binding Text}\" Style=\"{StaticResource PhoneTextNormalStyle}\" FontSize=\"24\" />" +
                "</DataTemplate>";
            DataTemplate tweetTemplate = (DataTemplate) XamlReader.Load(template);

            TweetsList.ItemTemplate = tweetTemplate;
            TweetsList.ItemsSource = TweetListViewModel.fromCollection(tweets).Tweets;
            TweetsPivot.Content = TweetsList;

            MainPivot.Items.Add(TweetsPivot);
        }

        protected void displayVideos(Collection<Video> videos) {
            VideosPivot = new PivotItem() { Name = "VideosPivot", Header = "videos" };

            VideosList = new ListBox() {
                Name = "VideosList",
                BorderThickness = new Thickness(0.0),
                BorderBrush = new SolidColorBrush(Colors.Transparent)
            };

            string template = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" +
                    "<TextBlock Text=\"{Binding Title}\" Style=\"{StaticResource PhoneTextNormalStyle}\" FontSize=\"24\" />" +
                "</DataTemplate>";
            DataTemplate videoTemplate = (DataTemplate) XamlReader.Load(template);

            VideosList.ItemTemplate = videoTemplate;
            VideosList.ItemsSource = VideoListViewModel.fromCollection(videos).Videos;
            VideosPivot.Content = VideosList;

            MainPivot.Items.Add(VideosPivot);
        }

        private void makeCall(object sender, MouseButtonEventArgs e) {
            PhoneCallTask call = new PhoneCallTask();
            call.PhoneNumber = view.legislator.phone;
            call.DisplayName = view.TitledName;
            call.Show();
        }

        private void visitWebsite(object sender, MouseButtonEventArgs e) {
            WebBrowserTask web = new WebBrowserTask();
            web.URL = view.legislator.website;
            web.Show();
        }

        private void NewsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (NewsList.SelectedIndex == -1)
                return;
        }

        private void TweetsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TweetsList.SelectedIndex == -1)
                return;
        }

        // plucks a youtube username from a url
        private string youtubeUsername(string url) {
            return "senatorkerry";
        }
    }
}