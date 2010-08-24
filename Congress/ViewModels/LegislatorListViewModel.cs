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
using Congress.Models;
using System.Linq;
using System.Collections.Generic;

namespace Congress {

    public class LegislatorListViewModel {
        public ObservableCollection<LegislatorViewModel> Legislators { get; set; }

        public LegislatorListViewModel() {
            Legislators = new ObservableCollection<LegislatorViewModel>() {};
        }

        public static LegislatorListViewModel fromCollection(Collection<Legislator> legislators) {
            IOrderedEnumerable<Legislator> sortedLegislators = legislators.OrderBy(l => l.title, new TitleComparer()).ThenBy(l => l.lastName);


            ObservableCollection<LegislatorViewModel> models = new ObservableCollection<LegislatorViewModel>();
            foreach (Legislator legislator in sortedLegislators) {
                models.Add(LegislatorViewModel.fromLegislator(legislator));
            }
            return new LegislatorListViewModel() { Legislators = models };
        }

        private class TitleComparer : Comparer<string> {
            public override int Compare(string x, string y) {
                if (x == y)
                    return 0;
                else if (x == "Sen")
                    return -1;
                else
                    return 1;
            }
        }
    }
}