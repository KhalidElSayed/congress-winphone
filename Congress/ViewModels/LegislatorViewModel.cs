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
        public string PhotoUrlMedium {get; set;}
        public string PhotoUrlLarge { get; set; }
        public string Phone {get; set;}
        public string Office {get; set;}
        public string CallMessage {get; set;}
        public string WebsiteMessage {get; set;}
        public string ShortWebsite {get; set;}
        public string NewsKeyword {get; set;}

        public Legislator legislator;

        public static LegislatorViewModel fromLegislator(Legislator legislator) {
            return new LegislatorViewModel() {
                legislator = legislator,
                OfficialName = getOfficialName(legislator),
                TitledName = titledName(legislator),
                Position = getPosition(legislator),
                Party = partyName(legislator),
                State = getStateName(legislator.state),
                District = getDistrict(legislator),
                Phone = legislator.phone,
                Office = getOffice(legislator),
                PhotoUrlMedium = photoUrl(PHOTO_MEDIUM, legislator.bioguideId),
                PhotoUrlLarge = photoUrl(PHOTO_LARGE, legislator.bioguideId),
                CallMessage = callMessage(legislator),
                WebsiteMessage = websiteMessage(legislator),
                ShortWebsite = shortWebsite(legislator),
                NewsKeyword = newsKeyword(legislator)
            };
        }

        public static string getName(Legislator legislator) {
            return commonName(legislator) + " " + legislator.lastName;
        }

        public static string commonName(Legislator legislator) {
            if (legislator.nickName != null && legislator.nickName.Length > 0)
                return legislator.nickName;
            else
                return legislator.firstName;
        }

        public static string titledName(Legislator legislator) {
            string name = legislator.title + ". " + getName(legislator);
            if (legislator.nameSuffix != null && !legislator.nameSuffix.Equals(""))
                name += ", " + legislator.nameSuffix;
            return name;
        }

        public static string getOfficialName(Legislator legislator) {
            return legislator.lastName + ", " + commonName(legislator);
        }

        public static string fullTitle(Legislator legislator) {
            if (legislator.title.Equals("Del"))
                return "Delegate";
            else if (legislator.title.Equals("Com"))
                return "Resident Commissioner";
            else if (legislator.title.Equals("Sen"))
                return "Senator";
            else // "Rep"
                return "Representative";
        }

        public static string getDomain(Legislator legislator) {
            if (legislator.district.Equals("Senior Seat") || legislator.district.Equals("Junior Seat"))
                return legislator.district;
            else if (legislator.district.Equals("0"))
                return "At-Large";
            else
                return "District " + legislator.district;
        }

        public static string partyName(Legislator legislator) {
            if (legislator.party.Equals("D"))
                return "Democrat";
            if (legislator.party.Equals("R"))
                return "Republican";
            if (legislator.party.Equals("I"))
                return "Independent";
            else
                return "";
        }

        public static string newsKeyword(Legislator legislator) {
            return legislator.title + ". " + getName(legislator);
        }

        public static string shortWebsite(Legislator legislator) {
            string website = legislator.website;
            website = Regex.Replace(website, "^http://(?:www\\.)?", "");
            website = Regex.Replace(website, "/$", "");
            return website;
        }

        public static string callMessage(Legislator legislator) {
            if (legislator.gender.Equals("M"))
                return "Call his office";
            else if (legislator.gender.Equals("F"))
                return "Call her office";
            else // safeguard against future transgendered or non-gendered officeholders
                return "Call their office";
        }

        public static string websiteMessage(Legislator legislator) {
            if (legislator.gender.Equals("M"))
                return "Visit his website";
            else if (legislator.gender.Equals("F"))
                return "Visit her website";
            else // safeguard against future transgendered or non-gendered officeholders
                return "Visit their website";
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
                    position = fullTitle(legislator) + " for " + state;
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