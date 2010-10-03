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

namespace Congress.ViewModels {

    public class CommitteeListViewModel {
        public ObservableCollection<CommitteeViewModel> Committees { get; set; }

        public CommitteeListViewModel() {
            Committees = new ObservableCollection<CommitteeViewModel>() {};
        }

        public static CommitteeListViewModel fromCollection(Collection<Committee> committees) {
            IOrderedEnumerable<Committee> sortedCommittees = committees.OrderBy(c => c.name, new NameComparer());

            ObservableCollection<CommitteeViewModel> models = new ObservableCollection<CommitteeViewModel>();
            foreach (Committee committee in sortedCommittees) {
                models.Add(CommitteeViewModel.fromCommittee(committee));
            }

            return new CommitteeListViewModel() { Committees = models };
        }

        private class NameComparer : Comparer<string> {
            public override int Compare(string x, string y) {
                string mine = x.Replace("the ", "");
                string other = y.Replace("the ", "");
                return mine.CompareTo(other);
            }
        }
    }
}