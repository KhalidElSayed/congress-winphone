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
using System.Text.RegularExpressions;

namespace Congress {
    public partial class LegislatorPivot : PhoneApplicationPage {
        private LegislatorViewModel view;

        public LegislatorPivot() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            string titledName, bioguideId;
            NavigationContext.QueryString.TryGetValue("titledName", out titledName);
            NavigationContext.QueryString.TryGetValue("bioguideId", out bioguideId);

            (ProfileSpinner.FindName("LoadingText") as TextBlock).Text = "Loading legislator...";
            ProfilePanel.Visibility = Visibility.Collapsed;
            ProfileSpinner.Visibility = Visibility.Visible;

            (NewsSpinner.FindName("LoadingText") as TextBlock).Text = "Plucking news from the air...";
            NewsList.Visibility = Visibility.Collapsed;
            NewsSpinner.Visibility = Visibility.Visible;

            (TweetsSpinner.FindName("LoadingText") as TextBlock).Text = "Plucking tweets from the air...";
            TweetsList.Visibility = Visibility.Collapsed;
            TweetsSpinner.Visibility = Visibility.Visible;

            (VideosSpinner.FindName("LoadingText") as TextBlock).Text = "Plucking videos from the air...";
            VideosList.Visibility = Visibility.Collapsed;
            VideosSpinner.Visibility = Visibility.Visible;

            MainPivot.Title = titledName;
            Legislator.find(bioguideId, new Legislator.LegislatorFoundEventHandler(displayLegislator));
        }

        protected void displayLegislator(Legislator legislator) {
            ProfileSpinner.Visibility = Visibility.Collapsed;
            ProfilePanel.Visibility = Visibility.Visible;

            this.view = LegislatorViewModel.fromLegislator(legislator);
            DataContext = view;
            
            // trigger news fetching
            NewsItem.search(view.NewsKeyword, displayNews);

            // if twitter_id then add pivot and trigger tweet fetching
            if (view.legislator.twitterId != null && view.legislator.twitterId.Length > 0)
                Tweet.search(legislator.twitterId, displayTweets);

            // if youtube_url then add pivot and trigger video fetching
            if (view.legislator.youtubeUrl != null && view.legislator.youtubeUrl.Length > 0)
                Video.getVideos(youtubeUsername(legislator.youtubeUrl), displayVideos);

            // committee fetching
            Committee.allForLegislator(view.legislator.bioguideId, displayCommittees);
        }

        protected void displayNews(Collection<NewsItem> items) {
            NewsSpinner.Visibility = Visibility.Collapsed;
            NewsList.Visibility = Visibility.Visible;
            NewsPivot.DataContext = NewsItemListViewModel.fromCollection(items);
        }

        protected void displayTweets(Collection<Tweet> tweets) {
            TweetsSpinner.Visibility = Visibility.Collapsed;
            TweetsList.Visibility = Visibility.Visible;
            TweetsPivot.DataContext = TweetListViewModel.fromCollection(tweets);
        }

        protected void displayVideos(Collection<Video> videos) {
            VideosSpinner.Visibility = Visibility.Collapsed;
            VideosList.Visibility = Visibility.Visible;
            VideosPivot.DataContext = VideoListViewModel.fromCollection(videos);
        }

        protected void displayCommittees(Collection<Committee> committees) {
            CommitteesPivot.DataContext = CommitteeListViewModel.fromCollection(committees);
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
            if (NewsList.SelectedIndex > -1)
                new WebBrowserTask() {URL = ((NewsItemViewModel) NewsList.SelectedItem).newsItem.url}.Show();
        }

        private void TweetsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // no action
        }

        private void CommitteesList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (CommitteesList.SelectedIndex == -1)
                return;

            CommitteeViewModel model = (CommitteesList.ItemsSource as ObservableCollection<CommitteeViewModel>)[CommitteesList.SelectedIndex];
            NavigationService.Navigate(new Uri("/LegislatorListPage.xaml?searchType=" + MainPage.SEARCH_COMMITTEE + "&committeeId=" + model.committee.id + "&committeeName=" + model.Name, UriKind.Relative));

            CommitteesList.SelectedIndex = -1;
        }

        private void VideosList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (VideosList.SelectedIndex == -1)
                return;
            
            VideoViewModel model = (VideoViewModel) VideosList.SelectedItem;
            Video video = model.video;

            WebBrowserTask task = new WebBrowserTask();
            task.URL = video.url;
            task.Show();
        }

        // plucks a youtube username from a url
        private string youtubeUsername(string url) {
            return Regex.Replace(url, "^http://(?:www\\.)?youtube\\.com/(?:user/)?(.*?)/?", "");
        }

        private void gotoBioguide(object s, EventArgs e) {
            new WebBrowserTask() { URL = view.BioguideUrl }.Show();
        }

        private void gotoOpenCongress(object s, EventArgs e) {
            new WebBrowserTask() { URL = view.OpenCongressUrl }.Show();
        }

        private void gotoGovTrack(object s, EventArgs e) {
            new WebBrowserTask() { URL = view.GovTrackUrl }.Show();
        }
    }
}