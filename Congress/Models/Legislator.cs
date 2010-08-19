using System;
using System.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Newtonsoft.Json.Linq;


namespace Congress.Models {
    public class Legislator {

        public string bioguideId;
        public string title, firstName, nickName, lastName, nameSuffix;

        public Legislator() {}

        public static void find(string bioguideId, LegislatorFoundEventHandler handler) {
            Uri uri = new Uri(Sunlight.url("legislators.get", "bioguide_id=" + bioguideId));
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                string json = e.Result;
                Legislator legislator = oneFromJSON(json);
                handler.Invoke(legislator);
            };
            downloader.DownloadStringAsync(uri);
        }

        public delegate void LegislatorFoundEventHandler(Legislator legislator);

        public static Legislator oneFromJSON(string json) {
            Legislator legislator = new Legislator();
            
            JObject root = JObject.Parse(json);
            root = (JObject) root["response"];
            root = (JObject) root["legislator"];

            legislator.bioguideId = (string) root["bioguide_id"];
            legislator.title = (string) root["title"];
            legislator.firstName = (string) root["firstname"];
            legislator.lastName = (string) root["lastname"];
            legislator.nickName = (string) root["nickname"];
            legislator.nameSuffix = (string) root["name_suffix"];

            return legislator;
        }

        public static Collection<Legislator> manyFromJSON(string json) {

            return null;
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