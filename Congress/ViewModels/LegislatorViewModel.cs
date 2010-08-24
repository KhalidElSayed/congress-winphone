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

namespace Congress {

    public class LegislatorViewModel {
        public static string PHOTO_LARGE = "200x250";
        public static string PHOTO_MEDIUM = "100x125";
        public static string PHOTO_SMALL = "40x50";

        public string OfficialName { get; set; }
        public string TitledName {get; set;}
        public string Position { get; set; }
        public string Party {get; set;}
        public string BioguideId { get; set; }
        public string PhotoUrl {get; set;}

        public static LegislatorViewModel fromLegislator(Legislator legislator) {
            return new LegislatorViewModel() {
                OfficialName = legislator.getOfficialName(),
                TitledName = legislator.titledName(),
                Position = getPosition(legislator),
                Party = legislator.partyName(),
                BioguideId = legislator.bioguideId,
                PhotoUrl = photoUrl(PHOTO_MEDIUM, legislator.bioguideId)
            };
        }

        public static string photoUrl(string size, string bioguideId) {
            return "http://assets.sunlightfoundation.com/moc/" + size + "/" + bioguideId + ".jpg";
        }

        public static string getPosition(Legislator legislator) {
            string district = legislator.district;
            string state = legislator.state;
            string position = "";
            
            if (district.Equals("Senior Seat"))
                position = "Senior Senator from " + state;
            else if (district.Equals("Junior Seat"))
                position = "Junior Senator from " + state;
            else if (district.Equals("0"))
            {
                if (legislator.title.Equals("Rep"))
                    position = "Representative for " + state + " At-Large";
                else
                    position = legislator.fullTitle() + " for " + state;
            }
            else
                position = "Representative for " + state + "-" + district;

            return position;
        }
    }
}