using System;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Congress {

    public class LegislatorListViewModel {
        public ObservableCollection<LegislatorViewModel> Legislators { get; set; }

        public LegislatorListViewModel() {
            Legislators = new ObservableCollection<LegislatorViewModel>() {};
        }
    }
}