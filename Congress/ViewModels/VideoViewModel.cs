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
    public class VideoViewModel {
        public string Title {get; set;}
        public string Description {get; set;}
        public string Date {get; set;}
        public string Url {get; set;}
        public string ThumbnailUrl {get; set;}

        public Video video;

        public static VideoViewModel fromVideo(Video video) {
            return new VideoViewModel() {
                Title = video.title,
                Description = video.description,
                Date = dateFor(video.updated),
                Url = video.url,
                ThumbnailUrl = video.thumbnailUrl
            };
        }

        public static String dateFor(DateTime updated) {
            return "___ __";
        }
    }
}
