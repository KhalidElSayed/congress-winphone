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
                Description = descriptionFor(video.description),
                Date = dateFor(video.updated),
                Url = video.url,
                ThumbnailUrl = video.thumbnailUrl,
                video = video
            };
        }

        public static string dateFor(DateTime updated) {
            if (DateTime.Now.Year == updated.Year)
                return String.Format("{0:MMM d}", updated);
            else
                return String.Format("{0:MMM d, yyyy}", updated);
        }

        public static string descriptionFor(string description) {
            if (description.Length > 150)
                return description.Remove(147) + "...";
            else
                return description;
        }
    }
}
