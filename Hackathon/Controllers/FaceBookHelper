using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Net;


namespace Hackathon.Controllers
{
    public class FaceBookHelper
    {

        
        public class Friend
        {
            public string id;
            public string name;
        }

        public class CheckIn
        {
            public string id;
            public DateTime created_time;
            public Friend from;
            public string type;
            //public List<Friend> likes;
            public Place place;
        }

        public class Place
        {
            public string id;
            public string name;
            public Location location;
        }

        public class Location
        {
            public string street;
            public string city;
            public string state;
            public string country;
            public string zip;
            public decimal latitude;
            public decimal longitude;
        }

        private string _id;
        private string _authToken;

        public FaceBookHelper(string AuthToken, string Id)
        {
            _id = Id;
            _authToken = AuthToken;
        }

        public List<Friend> GetFriends()
        {
            var webClient = new WebClient();

            
            var response = webClient.DownloadData(string.Format("https://graph.facebook.com/1207352884/friends?access_token={0}", _authToken));

            var anonymousTypeToReturn = new { data = new List<Friend>() };

            var resultOfConversion = JsonConvert.DeserializeAnonymousType(System.Text.Encoding.Default.GetString(response), anonymousTypeToReturn);

            return resultOfConversion.data;
        }

        public List<CheckIn> GetCheckins()
        {
            var webClient = new WebClient();

            var friends = GetFriends();

            var anonymousTypeToReturn = new { data = new List<CheckIn>() };

            var checkIns = new List<CheckIn>();

            foreach(Friend friend in friends)
            {
                string url = string.Format("https://graph.facebook.com/{0}/checkins?since={1}&access_token={2}&fields=created_time,id,from,place", friend.id, GetUnixTime(DateTime.Today.AddDays(-160)).ToString(), _authToken);
                var response = webClient.DownloadData(url);
                var resultOfConversion = JsonConvert.DeserializeAnonymousType(System.Text.Encoding.Default.GetString(response), anonymousTypeToReturn);

                checkIns.AddRange(resultOfConversion.data);

            }

            return checkIns;
            
        }

        double GetUnixTime(DateTime DateTimeConvert)
        {
            return (DateTimeConvert - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }


    }
}
