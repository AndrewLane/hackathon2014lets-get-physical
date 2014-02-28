using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon.CacheHelper;
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

        
        public class RankedFriend : Friend
        {
            public int physicalRank;
            public int virtualRank;

            public RankedFriend(Friend friend, int VirtualRank, int PhysicalRank)
            {
                id = friend.id;
                name = friend.name;
                physicalRank = PhysicalRank;
                virtualRank = VirtualRank;
            }
        }

        public class Status
        {
            public string id;
            public DateTime updated_item;
            public LikeData likes;
        }

        public class LikeData
        {
            public List<Friend> data;
        }

        private string _id;
        private string _authToken;
        private Dictionary<string, int> _tempFriendVirtualRankData;
        private Dictionary<string, int> _tempFriendPhysicalRankData;

        public FaceBookHelper(string AuthToken, string Id)
        {
            _id = Id;
            _authToken = AuthToken;
            _tempFriendVirtualRankData = new Dictionary<string, int>();
            _tempFriendPhysicalRankData = new Dictionary<string, int>();
        }

        public List<T> ExecuteApiCall<T>(string url)
        {
            var cacheKey = url;
            return HttpContext.Current.Cache.GetOrAdd(cacheKey, () =>
            {
                var webClient = new WebClient();

                var response = webClient.DownloadData(url);

                var anonymousTypeToReturn = new {data = new List<T>()};

                var resultOfConversion =
                    JsonConvert.DeserializeAnonymousType(System.Text.Encoding.Default.GetString(response),
                        anonymousTypeToReturn);

                return resultOfConversion.data;
            });

        }

        public List<Friend> GetFriends()
        {
            return ExecuteApiCall<Friend>(string.Format("https://graph.facebook.com/{1}/friends?access_token={0}", _authToken, _id));
        }


        private List<Status> GetStatusList()
        {
            return ExecuteApiCall<Status>(string.Format("https://graph.facebook.com/{0}/statuses?fields=id,likes&until=1384899552&access_token={1}", _id, _authToken));
        }

        private List<T> ExecutelFQL<T>(string FQL)
        {
            string fql = System.Web.HttpUtility.UrlEncode(FQL);

            return ExecuteApiCall<T>(string.Format("https://graph.facebook.com/fql?q={0}&access_token={1}",fql, _authToken));

        }

        private List<FriendPhoto> GetFriendPhotosImTaggedIn()
        {
            return ExecutelFQL<FriendPhoto>("SELECT owner FROM photo WHERE owner in (select uid2 from friend where uid1 = me())  AND object_id IN (SELECT object_id FROM photo_tag WHERE subject=me() AND created > " + GetUnixTime(DateTime.Today.AddDays(-160)).ToString() + " )");
        }

        private class FriendPhoto
        {
            public string owner;
        }

        public List<RankedFriend> GetRankedFriends()
        {
            var friends = GetFriends();

            foreach(var friend in friends)
            {
                _tempFriendVirtualRankData[friend.id] = 0;
                _tempFriendPhysicalRankData[friend.id] = 0;
            }

            var photoFriends = GetFriendPhotosImTaggedIn();
            foreach(var photoFriend in photoFriends)
            {
                if (_tempFriendPhysicalRankData.ContainsKey(photoFriend.owner))
                    _tempFriendPhysicalRankData[photoFriend.owner] += 1;
            }


            var Statuses = GetStatusList();
            foreach(var stat in Statuses)
            {
               if(stat.likes != null)
               {
                   foreach (var friendLike in stat.likes.data)
                   {
                       if (_tempFriendVirtualRankData.ContainsKey(friendLike.id))
                           _tempFriendVirtualRankData[friendLike.id] += 1;
                   }
               }
               
            }

            var rankedFriends = friends.ConvertAll((x) => new RankedFriend(x, _tempFriendVirtualRankData[x.id], _tempFriendPhysicalRankData[x.id]));
            
            return rankedFriends;
        }


        public List<CheckIn> GetCheckins()
        {
            var webClient = new WebClient();

            var friends = GetFriends();

            var anonymousTypeToReturn = new { data = new List<CheckIn>() };

            var totalCheckIns = new List<CheckIn>();

            foreach(Friend friend in friends)
            {
                string url = string.Format("https://graph.facebook.com/{0}/checkins?since={1}&access_token={2}&fields=created_time,id,from,place", friend.id, GetUnixTime(DateTime.Today.AddDays(-160)).ToString(), _authToken);

                var friendCheckIns = ExecuteApiCall<CheckIn>(url);

                totalCheckIns.AddRange(friendCheckIns);

            }

            return totalCheckIns;
            
        }

        public static double GetUnixTime(DateTime DateTimeConvert)
        {
            return (DateTimeConvert - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }


    }
}
