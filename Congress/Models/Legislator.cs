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


        /** Network operations */

        public static void findByState(string state, LegislatorsFoundEventHandler handler) {
            findMany("getList", "state=" + state, handler);
        }

        public static void findByZip(string zip, LegislatorsFoundEventHandler handler) {
            findMany("allForZip", "zip=" + zip, handler);
        }

        public static void findByLastName(string lastName, LegislatorsFoundEventHandler handler) {
            findMany("getList", "lastname=" + lastName, handler);
        }

        public static void findByLocation(string latitude, string longitude, LegislatorsFoundEventHandler handler) {
            findMany("allForLatLong", "latitude=" + latitude + "&longitude=" + longitude, handler);
        }

        public static void find(string bioguideId, LegislatorFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                handler.Invoke(oneFromJSON(e.Result));
            };

            downloader.DownloadStringAsync(new Uri(Sunlight.url("legislators.get", "bioguide_id=" + bioguideId)));
        }

        public static void findMany(string method, string queryString, LegislatorsFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                handler.Invoke(manyFromJSON(e.Result));
            };

            downloader.DownloadStringAsync(new Uri(Sunlight.url("legislators." + method, queryString)));
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