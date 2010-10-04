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
    public class Video {

        public string title, description, url, thumbnailUrl;
        public DateTime updated;

        public Video() {}

        public delegate void VideosFoundEventHandler(Collection<Video> videos);

        public static void getVideos(string username, VideosFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                try {
                    handler.Invoke(manyFromJSON(e.Result));
                } catch(WebException ex) {
                    handler.Invoke(null);
                }
            };

            String url = YouTube.videosUrl(username);
            downloader.DownloadStringAsync(new Uri(url));
        }

        public static Collection<Video> manyFromJSON(string json) {
            JObject root = JObject.Parse(json);
            JObject data = (JObject) root["data"];
            JArray items = (JArray) data["items"];

            Collection<Video> videos = new Collection<Video>();

            foreach (JToken video in items)
                videos.Add(oneFromJObject((JObject)video));

            return videos;
        }

        public static Video oneFromJObject(JObject root) {
            Video video = new Video();

            video.title = (string) root["title"];
            video.description = (string)root["description"];
            
            JObject player = (JObject) root["player"];
            video.url = (string)player["default"];

            JObject thumbnail = (JObject)root["thumbnail"];
            video.thumbnailUrl = (string)thumbnail["sqDefault"];

            string timestamp = (string)root["updated"];
            video.updated = DateTime.Parse(timestamp);

            return video;
        }

        public class YouTube {
            public static string BaseUrl = "http://gdata.youtube.com/feeds/api/";

            public static string videosUrl(string username) {
                return BaseUrl + "users/" + username + "/uploads" +
                    "?" +
                    "&alt=jsonc" +
                    "&v=2" +
                    "&orderby=updated";
            }
        }

        public static DateTime parseYouTubeTimestamp(string timestamp) {
            return DateTime.ParseExact(timestamp, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
        }
    }
}