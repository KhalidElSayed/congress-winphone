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
    public class VideoListViewModel {
        public ObservableCollection<VideoViewModel> Videos { get; set; }

        public VideoListViewModel() {
            Videos = new ObservableCollection<VideoViewModel>() {};
        }

        public static VideoListViewModel fromCollection(Collection<Video> videos) {

            ObservableCollection<VideoViewModel> models = new ObservableCollection<VideoViewModel>();
            foreach (Video video in videos)
                models.Add(VideoViewModel.fromVideo(video));

            return new VideoListViewModel() { Videos = models };
        }
    }
}
