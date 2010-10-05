using System;
using System.Linq;
using System.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Newtonsoft.Json.Linq;


namespace Congress.Models {
    public class Legislator {

        public string bioguideId, govTrackId; // unique ID
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
                try {
                    handler.Invoke(oneFromJSON(e.Result));
                } catch (WebException ex) {
                    handler.Invoke(null);
                }
            };

            downloader.DownloadStringAsync(new Uri(Sunlight.url("legislators.get", "bioguide_id=" + bioguideId)));
        }

        public static void findMany(string method, string queryString, LegislatorsFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                try {
                    handler.Invoke(manyFromJSON(e.Result));
                } catch (WebException ex) {
                    handler.Invoke(null);
                }
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
            legislator.govTrackId = (string) root["govtrack_id"];

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

        public static string stateCodeToName(string code) {
            code = code.ToUpper();
            if (code == "AL")
                return "Alabama";
            if (code == "AK")
                return "Alaska";
            if (code == "AS")
                return "American Samoa";
            if (code == "AZ")
                return "Arizona";
            if (code == "AR")
                return "Arkansas";
            if (code == "CA")
                return "California";
            if (code == "CO")
                return "Colorado";
            if (code == "CT")
                return "Connecticut";
            if (code == "DE")
                return "Delaware";
            if (code == "DC")
                return "District of Columbia";
            if (code == "FL")
                return "Florida";
            if (code == "GA")
                return "Georgia";
            if (code == "GU")
                return "Guam";
            if (code == "HI")
                return "Hawaii";
            if (code == "ID")
                return "Idaho";
            if (code == "IL")
                return "Illinois";
            if (code == "IN")
                return "Indiana";
            if (code == "IA")
                return "Iowa";
            if (code == "KS")
                return "Kansas";
            if (code == "KY")
                return "Kentucky";
            if (code == "LA")
                return "Louisiana";
            if (code == "ME")
                return "Maine";
            if (code == "MD")
                return "Maryland";
            if (code == "MA")
                return "Massachusetts";
            if (code == "MI")
                return "Michigan";
            if (code == "MN")
                return "Minnesota";
            if (code == "MS")
                return "Mississippi";
            if (code == "MO")
                return "Missouri";
            if (code == "MT")
                return "Montana";
            if (code == "NE")
                return "Nebraska";
            if (code == "NV")
                return "Nevada";
            if (code == "NH")
                return "New Hampshire";
            if (code == "NJ")
                return "New Jersey";
            if (code == "NM")
                return "New Mexico";
            if (code == "NY")
                return "New York";
            if (code == "NC")
                return "North Carolina";
            if (code == "ND")
                return "North Dakota";
            if (code == "MP")
                return "Northern Mariana Islands";
            if (code == "OH")
                return "Ohio";
            if (code == "OK")
                return "Oklahaoma";
            if (code == "OR")
                return "Oregon";
            if (code == "PA")
                return "Pennsylvania";
            if (code == "PR")
                return "Puerto Rico";
            if (code == "RI")
                return "Rhode Island";
            if (code == "SC")
                return "South Carolina";
            if (code == "SD")
                return "South Dakota";
            if (code == "TN")
                return "Tennessee";
            if (code == "TX")
                return "Texas";
            if (code == "UT")
                return "Utah";
            if (code == "VT")
                return "Vermont";
            if (code == "VI")
                return "Virgin Islands";
            if (code == "VA")
                return "Virginia";
            if (code == "WA")
                return "Washington";
            if (code == "WV")
                return "West Virginia";
            if (code == "WI")
                return "Wisconsin";
            if (code == "WY")
                return "Wyoming";
            else
                return null;
        }

        public static string stateNameToCode(string name) {
            if (name == "Alabama")
                return "AL";
            if (name == "Alaska")
                return "AK";
            if (name == "American Samoa")
                return "AS";
            if (name == "Arizona")
                return "AZ";
            if (name == "Arkansas")
                return "AR";
            if (name == "California")
                return "CA";
            if (name == "Colorado")
                return "CO";
            if (name == "Connecticut")
                return "CT";
            if (name == "Delaware")
                return "DE";
            if (name == "District of Columbia")
                return "DC";
            if (name == "Florida")
                return "FL";
            if (name == "Georgia")
                return "GA";
            if (name == "Guam")
                return "GU";
            if (name == "Hawaii")
                return "HI";
            if (name == "Idaho")
                return "ID";
            if (name == "Illinois")
                return "IL";
            if (name == "Indiana")
                return "IN";
            if (name == "Iowa")
                return "IA";
            if (name == "Kansas")
                return "KS";
            if (name == "Kentucky")
                return "KY";
            if (name == "Louisiana")
                return "LA";
            if (name == "Maine")
                return "ME";
            if (name == "Maryland")
                return "MD";
            if (name == "Massachusetts")
                return "MA";
            if (name == "Michigan")
                return "MI";
            if (name == "Minnesota")
                return "MN";
            if (name == "Mississippi")
                return "MS";
            if (name == "Missouri")
                return "MO";
            if (name == "Montana")
                return "MT";
            if (name == "Nebraska")
                return "NE";
            if (name == "Nevada")
                return "NV";
            if (name == "New Hampshire")
                return "NH";
            if (name == "New Jersey")
                return "NJ";
            if (name == "New Mexico")
                return "NM";
            if (name == "New York")
                return "NY";
            if (name == "North Carolina")
                return "NC";
            if (name == "North Dakota")
                return "ND";
            if (name == "Northern Mariana Islands")
                return "MP";
            if (name == "Ohio")
                return "OH";
            if (name == "Oklahaoma")
                return "OK";
            if (name == "Oregon")
                return "OR";
            if (name == "Pennsylvania")
                return "PA";
            if (name == "Puerto Rico")
                return "PR";
            if (name == "Rhode Island")
                return "RI";
            if (name == "South Carolina")
                return "SC";
            if (name == "South Dakota")
                return "SD";
            if (name == "Tennessee")
                return "TN";
            if (name == "Texas")
                return "TX";
            if (name == "Utah")
                return "UT";
            if (name == "Vermont")
                return "VT";
            if (name == "Virgin Islands")
                return "VI";
            if (name == "Virginia")
                return "VA";
            if (name == "Washington")
                return "WA";
            if (name == "West Virginia")
                return "WV";
            if (name == "Wisconsin")
                return "WI";
            if (name == "Wyoming")
                return "WY";
            else
                return null;
        }
    }
}