﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Congress {
    public partial class About : PhoneApplicationPage {

        public About() {
            InitializeComponent();
        }

        private void launchEmail(object sender, MouseButtonEventArgs e) {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = Congress.Strings.EmailSubject;
            task.To = Congress.Strings.EmailTo;
            task.Show();
        }
    }
}