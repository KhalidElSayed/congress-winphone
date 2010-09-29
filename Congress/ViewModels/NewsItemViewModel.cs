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
        public string DateSource {get; set;}

        public NewsItem newsItem;

        public static NewsItemViewModel fromNewsItem(NewsItem item) {
            return new NewsItemViewModel() {
                Title = item.title,
                Excerpt = descriptionFor(item.summary),
                Url = item.clickUrl,
                DateSource = dateSourceFor(item.published, item.source),
                newsItem = item
            };
        }

        public static string dateSourceFor(DateTime published, string source) {
            return dateFor(published) + " - " + source;
        }

        public static string dateFor(DateTime published) {
            return String.Format("{0:MMM d, yyyy}", published);
        }

        public static string descriptionFor(string description) {
            if (description.Length > 200)
                return description.Remove(197) + "...";
            else
                return description;
        }
    }
}
