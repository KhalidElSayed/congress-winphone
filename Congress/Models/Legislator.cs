using System;
using System.Linq;
using System.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Newtonsoft.Json.Linq;


namespace Congress.Models {
    public class Legislator {

        public string bioguideId; // unique ID
        public string title, firstName, nickName, middleName, lastName, nameSuffix;
        public string district, state, party, gender, senateClass;
        public string website, phone, congressOffice;
        public string youtubeUrl, twitterId;
        public bool inOffice;

        public Legislator() {}


        public string getName() {
            return commonName() + " " + lastName;
        }

        public string commonName() {
            if (nickName != null && nickName.Length > 0)
                return nickName;
            else
                return firstName;
        }

        public string titledName() {
            string name = title + ". " + getName();
            if (nameSuffix != null && !nameSuffix.Equals(""))
                name += ", " + nameSuffix;
            return name;
        }

        public string getOfficialName() {
            return lastName + ", " + commonName();
        }

        public string fullTitle() {
            if (title.Equals("Del"))
                return "Delegate";
            else if (title.Equals("Com"))
                return "Resident Commissioner";
            else if (title.Equals("Sen"))
                return "Senator";
            else // "Rep"
                return "Representative";
        }

        public string getDomain() {
            if (district.Equals("Senior Seat") || district.Equals("Junior Seat"))
                return district;
            else if (district.Equals("0"))
                return "At-Large";
            else
                return "District " + district;
        }

        public string partyName() {
            if (party.Equals("D"))
                return "Democrat";
            if (party.Equals("R"))
                return "Republican";
            if (party.Equals("I"))
                return "Independent";
            else
                return "";
        }


        /** Network operations */

        public static void find(string bioguideId, LegislatorFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                handler.Invoke(oneFromJSON(e.Result));
            };

            downloader.DownloadStringAsync(new Uri(Sunlight.url("legislators.get", "bioguide_id=" + bioguideId)));
        }

        public static void findByState(string state, LegislatorsFoundEventHandler handler) {
            findMany("state=" + state, handler);
        }

        public static void findMany(string queryString, LegislatorsFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                handler.Invoke(manyFromJSON(e.Result));
            };

            downloader.DownloadStringAsync(new Uri(Sunlight.url("legislators.getList", queryString)));
        }

        public delegate void LegislatorFoundEventHandler(Legislator legislator);
        public delegate void LegislatorsFoundEventHandler(Collection<Legislator> legislators);

        public static Legislator oneFromJSON(string json) {
            JObject root = JObject.Parse(json);
            JObject response = (JObject) root["response"];
            JObject legislator = (JObject) response["legislator"];
            return oneFromJObject(legislator);
        }

        public static Collection<Legislator> manyFromJSON(string json) {
            JObject root = JObject.Parse(json);
            JObject response = (JObject) root["response"];
            JArray legislatorsRoot = (JArray) response["legislators"];

            Collection<Legislator> legislators = new Collection<Legislator>();
            foreach (JToken item in legislatorsRoot) {
                JObject itemRoot = (JObject) ((JObject)item)["legislator"];
                legislators.Add(oneFromJObject(itemRoot));
            }

            return legislators;
        }

        public static Legislator oneFromJObject(JObject root) {
            Legislator legislator = new Legislator();

            legislator.bioguideId = (string) root["bioguide_id"];

            legislator.title = (string) root["title"];
            legislator.firstName = (string) root["firstname"];
            legislator.nickName = (string) root["nickname"];
            legislator.middleName = (string) root["middlename"];
            legislator.lastName = (string) root["lastname"];
            legislator.nameSuffix = (string) root["name_suffix"];

            legislator.district = (string) root["district"];
            legislator.state = (string) root["state"];
            legislator.party = (string) root["party"];
            legislator.gender = (string) root["gender"];
            legislator.senateClass = (string) root["senate_class"];

            legislator.website = (string) root["website"];
            legislator.phone = (string) root["phone"];
            legislator.congressOffice = (string) root["congress_office"];
            
            legislator.youtubeUrl = (string) root["youtube_url"];
            legislator.twitterId = (string) root["twitter_id"];

            legislator.inOffice = (bool) root["in_office"];
            
            return legislator;
        }


        public class Sunlight {
            public static string BaseUrl = "http://services.sunlightlabs.com/api/";
            public static string ApiKey = Congress.Strings.SunlightApiKey;

            public static string url(string method, string queryString) {
                if (queryString.Length > 0)
                    queryString += "&";

                queryString += "apikey=" + ApiKey;
                return BaseUrl + method + ".json" + "?" + queryString;
            }
        }
    }
}