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

namespace Congress.ViewModels {
    public class NewsItemViewModel {
        public string Title {get; set;}
        public string Excerpt {get; set;}
        public string Url {get; set;}
        public string Source {get; set;}
        public string Timestamp {get; set;}

        //public NewsItem newsItem;
    }
}
