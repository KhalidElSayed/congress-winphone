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
    public class NewsItemViewModel {
        public string Title {get; set;}
        public string Excerpt {get; set;}
        public string Url {get; set;}
        public string Source {get; set;}
        public string Timestamp {get; set;}

        public NewsItem newsItem;

        public static NewsItemViewModel fromNewsItem(NewsItem item) {
            return new NewsItemViewModel() {
                Title = item.title,
                Excerpt = item.summary,
                Url = item.clickUrl,
                Source = item.source,
                Timestamp = "time",
                newsItem = item
            };
        }
    }
}
