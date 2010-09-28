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
        public string Username {get; set;}
        public string Text {get; set;}
        public string Date {get; set;}

        public Tweet tweet;

        public static TweetViewModel fromNewsItem(Tweet tweet) {
            return new TweetViewModel() {
                Username = tweet.username,
                Text = tweet.text,
                Date = dateFor(tweet)
            };
        }

        public static String dateFor(Tweet tweet) {
            return "_ days ago";
        }
    }
}
