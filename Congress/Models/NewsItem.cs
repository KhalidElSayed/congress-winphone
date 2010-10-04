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

namespace Congress.Models {
    public class NewsItem {

        public string title, source, summary, url, clickUrl;
        public DateTime published;

        public NewsItem() {}

        public delegate void NewsItemsFoundEventHandler(Collection<NewsItem> items);

        public static void search(string query, NewsItemsFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                try {
                    handler.Invoke(manyFromJSON(e.Result));
                } catch (WebException ex) {
                    handler.Invoke(null);
                }
            };

            String url = YahooNews.searchUrl(query);
            downloader.DownloadStringAsync(new Uri(url));
        }

        public static Collection<NewsItem> manyFromJSON(string json) {
            JObject root = JObject.Parse(json);
            JObject resultSet = (JObject) root["ResultSet"];
            JArray itemsRoot = (JArray) resultSet["Result"];

            Collection<NewsItem> items = new Collection<NewsItem>();
            foreach (JToken item in itemsRoot)
                items.Add(oneFromJObject((JObject) item));

            return items;
        }

        public static NewsItem oneFromJObject(JObject root) {
            NewsItem item = new NewsItem();

            item.title = (string)root["Title"];
            item.source = (string)root["NewsSource"];
            item.summary = (string)root["Summary"];
            item.url = (string)root["Url"];
            item.clickUrl = (string)root["ClickUrl"];

            string publishedTimestamp = (string) root["PublishDate"];
            long publishedLong = Int64.Parse(publishedTimestamp);
            item.published = EpochToDate(publishedLong);

            return item;
        }

        public class YahooNews {
            public static string BaseUrl = "http://search.yahooapis.com/NewsSearchService/V1/newsSearch";
            public static string ApiKey = Congress.Strings.YahooApiKey;

            public static string searchUrl(string query) {
                string type = "phrase";
		        string sort = "date";
		        string language = "en";
		        string output = "json";
		        int results = 10;

                return BaseUrl + "?"
                    + "type=" + type
                    + "&sort=" + sort
                    + "&language=" + language
                    + "&output=" + output
                    + "&results=" + results
                    + "&appid=" + ApiKey
                    + "&query=" + query;
            }
        }

        // Epoch helper methods (since .NET works with "ticks" and Yahoo uses epoch-based timestamps

        public static readonly DateTime JAN_01_1970 = 
            DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0), DateTimeKind.Utc);

        // Get Unix Timestamp for given DateTime
        public static long SecondsFromEpoch(DateTime date) {
            DateTime dt = date.ToUniversalTime();
            TimeSpan ts = dt.Subtract(JAN_01_1970);
            return (long)ts.TotalSeconds;
        }

        // Given Unix Timestamp, get DateTime
        public static DateTime EpochToDate(long secFromEpoch) {
            return JAN_01_1970.AddSeconds(secFromEpoch);
        }
    }
}