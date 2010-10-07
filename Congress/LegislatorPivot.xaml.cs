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

        Legislator legislator = null;
        Collection<NewsItem> items = null;
        Collection<Tweet> tweets = null;
        Collection<Video> videos = null;
        Collection<Committee> committees = null;


        public LegislatorPivot() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            string titledName, bioguideId;
            NavigationContext.QueryString.TryGetValue("titledName", out titledName);
            NavigationContext.QueryString.TryGetValue("bioguideId", out bioguideId);

            MainPivot.Title = titledName;

            object legislatorObject;
            if (State.TryGetValue("legislator", out legislatorObject))
                legislator = legislatorObject as Legislator;

            object itemsObject;
            if (State.TryGetValue("items", out itemsObject))
                items = itemsObject as Collection<NewsItem>;

            object tweetsObject;
            if (State.TryGetValue("tweets", out tweetsObject))
                tweets = tweetsObject as Collection<Tweet>;

            object videosObject;
            if (State.TryGetValue("videos", out videosObject))
                videos = videosObject as Collection<Video>;

            object committeesObject;
            if (State.TryGetValue("committees", out committeesObject))
                committees = committeesObject as Collection<Committee>;

            if (legislator == null) {
                ProfileMessage.Visibility = Visibility.Collapsed;
                MainPivot.Visibility = Visibility.Collapsed;
                ApplicationBar.IsVisible = false;
                (ProfileSpinner.FindName("LoadingText") as TextBlock).Text = "Loading legislator...";
                ProfileSpinner.Visibility = Visibility.Visible;
                Legislator.find(bioguideId, new Legislator.LegislatorFoundEventHandler(displayLegislator));
            } else
                displayLegislator(legislator);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);
            State["legislator"] = legislator;
            State["items"] = items;
            State["tweets"] = tweets;
            State["videos"] = videos;
            State["committees"] = committees;
        }

        protected void displayLegislator(Legislator legislator) {
            ProfileSpinner.Visibility = Visibility.Collapsed;
            ProfileMessage.Visibility = Visibility.Collapsed;
            this.legislator = legislator;

            if (legislator != null) {

                this.view = LegislatorViewModel.fromLegislator(legislator);
                DataContext = view;


                if (items == null) {
                    NewsMessage.Visibility = Visibility.Collapsed;
                    NewsList.Visibility = Visibility.Collapsed;
                    (NewsSpinner.FindName("LoadingText") as TextBlock).Text = "Plucking news from the air...";
                    NewsSpinner.Visibility = Visibility.Visible;
                    NewsItem.search(view.NewsKeyword, displayNews);
                } else
                    displayNews(items);

                if (tweets == null) {
                    if (view.legislator.twitterId != null && view.legislator.twitterId.Length > 0) {
                        TweetsMessage.Visibility = Visibility.Collapsed;
                        TweetsList.Visibility = Visibility.Collapsed;
                        (TweetsSpinner.FindName("LoadingText") as TextBlock).Text = "Plucking tweets from the air...";
                        TweetsSpinner.Visibility = Visibility.Visible;
                        Tweet.search(legislator.twitterId, displayTweets);
                    } else {
                        TweetsSpinner.Visibility = Visibility.Collapsed;
                        (TweetsMessage.FindName("Message") as TextBlock).Text = this.view.TitledName + " doesn't use Twitter.";
                        TweetsMessage.Visibility = Visibility.Visible;
                    }
                } else
                    displayTweets(tweets);

                if (videos == null) {
                    if (view.legislator.youtubeUrl != null && view.legislator.youtubeUrl.Length > 0) {
                        VideosMessage.Visibility = Visibility.Collapsed;
                        VideosList.Visibility = Visibility.Collapsed;
                        (VideosSpinner.FindName("LoadingText") as TextBlock).Text = "Plucking videos from the air...";
                        VideosSpinner.Visibility = Visibility.Visible;
                        Video.getVideos(youtubeUsername(legislator.youtubeUrl), displayVideos);
                    } else {
                        VideosSpinner.Visibility = Visibility.Collapsed;
                        (VideosMessage.FindName("Message") as TextBlock).Text = this.view.TitledName + " doesn't use YouTube.";
                        VideosMessage.Visibility = Visibility.Visible;
                    }
                } else
                    displayVideos(videos);

                if (committees == null) {
                    CommitteesMessage.Visibility = Visibility.Collapsed;
                    CommitteesList.Visibility = Visibility.Collapsed;
                    (CommitteesSpinner.FindName("LoadingText") as TextBlock).Text = "Plucking committees from the air...";
                    CommitteesSpinner.Visibility = Visibility.Visible;
                    Committee.allForLegislator(view.legislator.bioguideId, displayCommittees);
                } else
                    displayCommittees(committees);

                MainPivot.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = true;
                
            } else {
                (ProfileMessage.FindName("Message") as TextBlock).Text = "There was a problem loading information for " + this.view.TitledName + ".";
                ProfileMessage.Visibility = Visibility.Visible;
            }
        }

        protected void displayNews(Collection<NewsItem> items) {
            NewsSpinner.Visibility = Visibility.Collapsed;
            NewsMessage.Visibility = Visibility.Collapsed;
            this.items = items;

            if (items != null) {

                if (items.Count > 0) {
                    NewsList.Visibility = Visibility.Visible;
                    NewsPivot.DataContext = NewsItemListViewModel.fromCollection(items);
                } else {
                    (NewsMessage.FindName("Message") as TextBlock).Text = "There aren't any recent news mentions for " + this.view.TitledName + ".";
                    NewsMessage.Visibility = Visibility.Visible;
                }

            } else {
                (NewsMessage.FindName("Message") as TextBlock).Text = "There was a problem loading news mentions.";
                NewsMessage.Visibility = Visibility.Visible;
            }
        }

        protected void displayTweets(Collection<Tweet> tweets) {
            TweetsSpinner.Visibility = Visibility.Collapsed;
            TweetsMessage.Visibility = Visibility.Collapsed;
            this.tweets = tweets;

            if (tweets != null) {

                if (tweets.Count > 0) {
                    TweetsList.Visibility = Visibility.Visible;
                    TweetsPivot.DataContext = TweetListViewModel.fromCollection(tweets);
                } else {
                    // should probably never happen
                    (TweetsMessage.FindName("Message") as TextBlock).Text = this.view.TitledName + " uses Twitter, but doesn't have any tweets.";
                    TweetsMessage.Visibility = Visibility.Visible;
                }

            } else {
                (TweetsMessage.FindName("Message") as TextBlock).Text = "There was a problem loading tweets.";
                TweetsMessage.Visibility = Visibility.Visible;
            }
        }

        protected void displayVideos(Collection<Video> videos) {
            VideosSpinner.Visibility = Visibility.Collapsed;
            VideosMessage.Visibility = Visibility.Collapsed;
            this.videos = videos;

            if (videos != null) {
                if (videos.Count > 0) {
                    VideosList.Visibility = Visibility.Visible;
                    VideosPivot.DataContext = VideoListViewModel.fromCollection(videos);
                } else {
                    // should probably never happen
                    (VideosMessage.FindName("Message") as TextBlock).Text = this.view.TitledName + " uses YouTube, but doesn't have any videos.";
                    VideosMessage.Visibility = Visibility.Visible;
                }

            } else {
                (VideosMessage.FindName("Message") as TextBlock).Text = "There was a problem loading videos.";
                VideosMessage.Visibility = Visibility.Visible;
            }
        }

        protected void displayCommittees(Collection<Committee> committees) {
            CommitteesSpinner.Visibility = Visibility.Collapsed;
            CommitteesMessage.Visibility = Visibility.Collapsed;
            this.committees = committees;

            if (committees != null) {
                if (committees.Count > 0) {
                    CommitteesList.Visibility = Visibility.Visible;
                    CommitteesPivot.DataContext = CommitteeListViewModel.fromCollection(committees);
                } else {
                    (CommitteesMessage.FindName("Message") as TextBlock).Text = this.view.TitledName + " doesn't belong to any committees.";
                    CommitteesMessage.Visibility = Visibility.Visible;
                }

            } else {
                (CommitteesMessage.FindName("Message") as TextBlock).Text = "There was a problem loading committee information.";
                CommitteesMessage.Visibility = Visibility.Visible;
            }
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