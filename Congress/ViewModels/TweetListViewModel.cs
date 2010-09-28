using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Congress.Models;

namespace Congress.ViewModels {
    public class TweetListViewModel {
        public ObservableCollection<TweetViewModel> Tweets { get; set; }

        public TweetListViewModel() {
            Tweets = new ObservableCollection<TweetViewModel>() {};
        }

        public static TweetListViewModel fromCollection(Collection<Tweet> tweets) {

            ObservableCollection<TweetViewModel> models = new ObservableCollection<TweetViewModel>();
            foreach (Tweet tweet in tweets)
                models.Add(TweetViewModel.fromNewsItem(tweet));

            return new TweetListViewModel() { Tweets = models };
        }
    }
}
