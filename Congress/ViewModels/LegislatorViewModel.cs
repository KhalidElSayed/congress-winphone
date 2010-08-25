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

namespace Congress {

    public class LegislatorViewModel {
        public static string PHOTO_LARGE = "200x250";
        public static string PHOTO_MEDIUM = "100x125";
        public static string PHOTO_SMALL = "40x50";

        public string OfficialName { get; set; }
        public string TitledName {get; set;}
        public string Position { get; set; }
        public string Party {get; set;}
        public string District {get; set;}
        public string State {get; set;}
        public string BioguideId { get; set; }
        public string PhotoUrlMedium {get; set;}
        public string PhotoUrlLarge { get; set; }
        public string Phone {get; set;}
        public string Website {get; set;}
        public string Office {get; set;}

        public static LegislatorViewModel fromLegislator(Legislator legislator) {
            return new LegislatorViewModel() {
                OfficialName = legislator.getOfficialName(),
                TitledName = legislator.titledName(),
                Position = getPosition(legislator),
                Party = legislator.partyName(),
                State = getStateName(legislator.state),
                District = getDistrict(legislator),
                Phone = legislator.phone,
                Website = legislator.website,
                BioguideId = legislator.bioguideId,
                Office = getOffice(legislator),
                PhotoUrlMedium = photoUrl(PHOTO_MEDIUM, legislator.bioguideId),
                PhotoUrlLarge = photoUrl(PHOTO_LARGE, legislator.bioguideId),
            };
        }

        public static string photoUrl(string size, string bioguideId) {
            return "http://assets.sunlightfoundation.com/moc/" + size + "/" + bioguideId + ".jpg";
        }

        public static string getOffice(Legislator legislator) {
            string office = legislator.congressOffice;
            string shortOffice = Regex.Replace(office, "(?:House|Senate) (?:Office)? Building", String.Empty, RegexOptions.IgnoreCase);
            return shortOffice.Trim();
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

        public static string getDistrict(Legislator legislator) {
            if (legislator.district.Equals("0"))
                return "At-Large";
            else if (legislator.district.Equals("Senior Seat") || legislator.district.Equals("Junior Seat"))
                return legislator.district;
            else
                return "District " + legislator.district;
        }

        public static string getStateName(string stateCode) {
            //TODO: Expand to full state names
            return stateCode;
        }
    }
}