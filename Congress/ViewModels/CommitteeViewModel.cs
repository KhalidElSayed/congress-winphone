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
using Congress.Models;
using System.Text.RegularExpressions;

namespace Congress.ViewModels {

    public class CommitteeViewModel {
        public string Name {get; set;}
        public string Chamber {get; set;}
        // public ObservableCollection<Legislator> Members;

        public Committee committee;
        
        public static CommitteeViewModel fromCommittee(Committee committee) {
            return new CommitteeViewModel() {
                committee = committee,
                Name = committee.name,
                Chamber = committee.chamber
            };
        }
    }
}