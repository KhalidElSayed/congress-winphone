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
    public class NewsItemListViewModel {
        public ObservableCollection<NewsItemViewModel> NewsItems { get; set; }

        public NewsItemListViewModel() {
            NewsItems = new ObservableCollection<NewsItemViewModel>() {};
        }

        public static NewsItemListViewModel fromCollection(Collection<NewsItem> items) {

            ObservableCollection<NewsItemViewModel> models = new ObservableCollection<NewsItemViewModel>();
            foreach (NewsItem item in items)
                models.Add(NewsItemViewModel.fromNewsItem(item));

            return new NewsItemListViewModel() { NewsItems = models };
        }
    }
}
