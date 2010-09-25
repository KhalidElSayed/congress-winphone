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

namespace Congress.ViewModels {
    public class NewsItemListViewModel {
        public ObservableCollection<NewsItemViewModel> NewsItems { get; set; }

        public NewsItemListViewModel() {
            NewsItems= new ObservableCollection<NewsItemViewModel>() {};
        }
    }
}
