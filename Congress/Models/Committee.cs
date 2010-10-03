using System;
using System.Linq;
using System.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Newtonsoft.Json.Linq;


namespace Congress.Models {
    public class Committee {

        public string id, name, chamber;
        public Collection<Legislator> members;

        public Committee() {}


        /** Network operations */

        public static void allForLegislator(string bioguideId, CommitteesFoundEventHandler handler) {
            findMany("committees.allForLegislator", "bioguide_id=" + bioguideId, handler);
        }

        public static void allForChamber(string chamber, CommitteesFoundEventHandler handler) {
            // capitalize chamber
            char[] array = chamber.ToCharArray();
            array[0] = char.ToUpper(array[0]);
            chamber = new string(array);

            findMany("committees.getList", "chamber=" + chamber, handler);
        }

        public static void find(string id, CommitteeFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                handler.Invoke(oneFromJSON(e.Result));
            };

            string url = Sunlight.url("committees.get", "id=" + id);
            downloader.DownloadStringAsync(new Uri(url));
        }

        public static void findMany(string method, string queryString, CommitteesFoundEventHandler handler) {
            WebClient downloader = new WebClient();

            downloader.DownloadStringCompleted += (s, e) => {
                handler.Invoke(manyFromJSON(e.Result));
            };

            string url = Sunlight.url(method, queryString);
            downloader.DownloadStringAsync(new Uri(url));
        }

        public delegate void CommitteeFoundEventHandler(Committee committee);
        public delegate void CommitteesFoundEventHandler(Collection<Committee> committees);

        public static Committee oneFromJSON(string json) {
            JObject root = JObject.Parse(json);
            JObject response = (JObject) root["response"];
            JObject committee = (JObject) response["committee"];
            return oneFromJObject(committee);
        }

        public static Collection<Committee> manyFromJSON(string json) {
            JObject root = JObject.Parse(json);
            JObject response = (JObject) root["response"];
            JArray committeesRoot = (JArray) response["committees"];

            Collection<Committee> committees = new Collection<Committee>();
            foreach (JToken item in committeesRoot) {
                JObject itemRoot = (JObject) ((JObject)item)["committee"];
                committees.Add(oneFromJObject(itemRoot));
            }

            return committees;
        }

        public static Committee oneFromJObject(JObject root) {
            Committee committee = new Committee();
            
            committee.id = (string)root["id"];
            committee.name = (string)root["name"];
            committee.chamber = (string)root["chamber"];

            JToken membersToken = root["members"];
            if (membersToken != null) {
                committee.members = new Collection<Legislator>();

                JArray membersRoot = (JArray)membersToken;
                foreach(JToken item in membersRoot) {
                    JObject itemRoot = (JObject) ((JObject)item)["legislator"];
                    committee.members.Add(Legislator.oneFromJObject(itemRoot));
                }
            }

            return committee;
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