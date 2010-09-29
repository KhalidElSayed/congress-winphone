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
using Congress.Models;

namespace Congress.ViewModels {
    public class TweetViewModel {
        public string Text {get; set;}
        public string Byline {get; set;}

        public Tweet tweet;

        public static TweetViewModel fromTweet(Tweet tweet) {
            return new TweetViewModel() {
                Text = tweet.text,
                Byline = bylineFor(tweet.created_at, tweet.username),
                tweet = tweet
            };
        }

        public static string bylineFor(DateTime created_at, string username) {
            return "posted " + dateFor(created_at) + " by @" + username;
        }

        public static string dateFor(DateTime created_at) {
            DateTime now = new DateTime();
            int year = now.Year;
            return "_ days ago";
            //return String.Format("{0:MMM d, yyyy}", updated);
        }
    }
}
