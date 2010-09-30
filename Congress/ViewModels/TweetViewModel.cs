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
            DateTime now = DateTime.Now;
            TimeSpan gap = now.Subtract(created_at);
            int diff = (int) gap.TotalSeconds;

            if (diff <= 2) // 2 seconds
                return "just now";
            else if (diff <= 50) // 50 seconds
                return diff + " seconds ago";
            else if (diff <= 65) // 1 minute, 5 seconds
                return "a minute ago";
            else if (diff <= 3300) // 55 minutes
                return (diff / 60) + " minutes ago";
            else if (diff <= 3900) // 65 minutes
                return "an hour ago";
            else if (diff <= 82800) // 23 hours
                return (diff / 3600) + " hours ago";
            else if (diff <= 90000) // 25 hours
                return "a day ago";
            else if (diff <= 1123200) // 13 days
                return (diff / 86400) + " days ago";
            else {
                if (now.Year == created_at.Year)
                    return String.Format("{0:MMM d}", created_at);
                else
                    return String.Format("{0:MMM d, yyyy}", created_at);
            }
        }
    }
}
