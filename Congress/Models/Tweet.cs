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
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Congress.Models {
    public class Tweet {

        public string username, text;
        public DateTime created_at;

        public Tweet() {}

        public delegate void TweetsFoundEventHandler(Collection<Tweet> tweets);

        public static void search(string username, TweetsFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                try {
                    handler.Invoke(manyFromJSON(e.Result));
                } catch (Exception ex) {
                    handler.Invoke(null);
                }
            };

            String url = Twitter.timelineUrl(username);
            downloader.DownloadStringAsync(new Uri(url));
        }

        public static Collection<Tweet> manyFromJSON(string json) {
            JArray root = JArray.Parse(json);

            Collection<Tweet> tweets = new Collection<Tweet>();
            foreach (JToken tweet in root)
                tweets.Add(oneFromJObject((JObject) tweet));

            return tweets;
        }

        public static Tweet oneFromJObject(JObject root) {
            Tweet tweet = new Tweet();

            tweet.text = (string)root["text"];
            
            JObject user = (JObject)root["user"];
            tweet.username = (string) user["screen_name"];

            string publishedTimestamp = (string) root["created_at"];
            tweet.created_at = parseTwitterTimestamp(publishedTimestamp);

            return tweet;
        }

        public class Twitter {
            public static string BaseUrl = "http://api.twitter.com/1/";

            public static string timelineUrl(string username) {
                return BaseUrl + "statuses/user_timeline.json" +
                    "?" +
                    "screen_name=" + username;
            }
        }

        public static DateTime parseTwitterTimestamp(string timestamp) {
            return DateTime.ParseExact(timestamp, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
        }
    }
}