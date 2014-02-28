using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon.CacheHelper;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Hackathon.Models;


namespace Hackathon.Controllers
{
    public class FaceBookHelper
    {
        public const int c_BackNumberOfDays = -30;

        private string _id;
        private string _authToken;
        private Dictionary<string, RunningTotals> _tempRankData;

        public class FriendPhoto
        {
            public string owner;
        }

        public FaceBookHelper(string AuthToken, string Id)
        {
            _id = Id;
            _authToken = AuthToken;
            _tempRankData = new Dictionary<string, RunningTotals>();
        }

        #region Internal Methods
       
        protected List<T> ExecuteApiCall<T>(string url)
        {
            var cacheKey = url;
            return HttpContext.Current.Cache.GetOrAdd(cacheKey, () =>
            {
                var webClient = new WebClient();

                var response = webClient.DownloadData(url);

                var anonymousTypeToReturn = new { data = new List<T>() };

                var resultOfConversion =
                    JsonConvert.DeserializeAnonymousType(System.Text.Encoding.Default.GetString(response),
                        anonymousTypeToReturn);

                return resultOfConversion.data;
            });

        }

        protected dynamic ExecuteApiCall(string url)
        {
            var cacheKey = url;
            return HttpContext.Current.Cache.GetOrAdd(cacheKey, () =>
            {
                var webClient = new WebClient();

                var response = webClient.DownloadData(url);

                dynamic resultOfConversion = JsonConvert.DeserializeObject((System.Text.Encoding.Default.GetString(response)));

                return resultOfConversion;
            });
        }

        protected List<Friend> GetFriends()
        {
            var friends = ExecuteFQL<Friend>("SELECT uid, name, pic_big FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1 = me()) ");


            return friends;
        }

        protected List<Status> GetStatusList()
        {
            return ExecuteApiCall<Status>(string.Format("https://graph.facebook.com/{0}/statuses?fields=id,likes&until=1384899552&access_token={1}", _id, _authToken));
        }

        protected List<T> ExecuteFQL<T>(string FQL)
        {
            string fql = System.Web.HttpUtility.UrlEncode(FQL);

            return ExecuteApiCall<T>(string.Format("https://graph.facebook.com/fql?q={0}&access_token={1}", fql, _authToken));
        }

        protected dynamic ExecuteFQL(string FQL)
        {
            string fql = System.Web.HttpUtility.UrlEncode(FQL);

            return ExecuteApiCall(string.Format("https://graph.facebook.com/fql?q={0}&access_token={1}", fql, _authToken));
        }

        protected List<FriendPhoto> GetFriendPhotosImTaggedIn()
        {
            return ExecuteFQL<FriendPhoto>("SELECT owner FROM photo WHERE owner in (select uid2 from friend where uid1 = me())  AND object_id IN (SELECT object_id FROM photo_tag WHERE subject=me() AND created > " + GetUnixTime(DateTime.Today.AddDays(c_BackNumberOfDays)).ToString() + " )");
        }

        protected List<CheckinResult> GetFriendsWhoTaggedMeInCheckins()
        {
            string fql = string.Format("SELECT author_uid FROM checkin WHERE author_uid in (select uid2 from friend where uid1 = me()) AND timestamp > {0}  AND me() IN tagged_uids", GetUnixTime(DateTime.Today.AddDays(c_BackNumberOfDays)).ToString());

            return ExecuteFQL<CheckinResult>(fql);
        }

        protected List<CommentResult> GetFriendsWhoHaveCommented()
        {
            string fql = string.Format("select fromid, time from comment where post_id in (select post_id from stream where source_id = me()) AND time > {0}", GetUnixTime(DateTime.Today.AddDays(c_BackNumberOfDays)).ToString());
            return ExecuteFQL<CommentResult>(fql);
        }

        protected List<CheckIn> GetCheckins()
        {
            var webClient = new WebClient();

            var friends = GetFriends();

            var anonymousTypeToReturn = new { data = new List<CheckIn>() };

            var totalCheckIns = new List<CheckIn>();

            foreach (Friend friend in friends)
            {
                string url = string.Format("https://graph.facebook.com/{0}/checkins?since={1}&access_token={2}&fields=created_time,id,from,place", friend.uid, GetUnixTime(DateTime.Today.AddDays(c_BackNumberOfDays)).ToString(), _authToken);

                var friendCheckIns = ExecuteApiCall<CheckIn>(url);

                totalCheckIns.AddRange(friendCheckIns);

            }

            return totalCheckIns;

        }

        #endregion

        #region "Public Methods"

        public List<RankedFriend> GetRankedFriends()
        {
            var friends = GetFriends();

            foreach (var friend in friends)
            {
                _tempRankData[friend.uid] = new RunningTotals();
                _tempRankData[friend.uid].totalPhysicalRank = 0;
                _tempRankData[friend.uid].totalVirtualRank = 0;
                _tempRankData[friend.uid].lastDateTime = 0;

            }

            var photoFriends = GetFriendPhotosImTaggedIn();
            foreach (var photoFriend in photoFriends)
            {
                if (_tempRankData.ContainsKey(photoFriend.owner))
                {
                    _tempRankData[photoFriend.owner].totalPhysicalRank += 53;
                    
                }
            }


            var Statuses = GetStatusList();
            foreach (var stat in Statuses)
            {
                if (stat.likes != null)
                {
                    foreach (var friendLike in stat.likes.data)
                    {
                        if (_tempRankData.ContainsKey(friendLike.id))
                            _tempRankData[friendLike.id].totalVirtualRank  += 32;
                    }
                }

            }

            var checkIns = GetFriendsWhoTaggedMeInCheckins();
            foreach (var ci in checkIns)
            {
                if (_tempRankData.ContainsKey(ci.author_uid))
                    _tempRankData[ci.author_uid].totalPhysicalRank  += 28;

            }


            var commenters = GetFriendsWhoHaveCommented();
            foreach(var commenter in commenters)
            {
                if (_tempRankData.ContainsKey(commenter.fromid))
                {
                    _tempRankData[commenter.fromid].totalVirtualRank += 28;
                    if(_tempRankData[commenter.fromid].lastDateTime < commenter.time )
                        _tempRankData[commenter.fromid].lastDateTime =  commenter.time;
                }
            }

            var rankedFriends = friends.ConvertAll((x) => new RankedFriend(x, _tempRankData[x.uid].totalVirtualRank , _tempRankData[x.uid].totalPhysicalRank,  UnixTimeStampToDateTime(_tempRankData[x.uid].lastDateTime)));

            return rankedFriends;
        }
        
        public FriendExtendedInfo GetFriendExtendedInfo(string uid)
        {
            var friendInfo = new FriendExtendedInfo();

            LastMessage lm = ExecuteFQL<LastMessage>("SELECT status_id, uid, message, time FROM status WHERE uid = " + uid + " ORDER BY time DESC LIMIT 1").FirstOrDefault();
            dynamic lc = ExecuteFQL("SELECT birthday, current_location from user where uid = " + uid);
            
            if (lm != null)
            {
                friendInfo.message = lm.message;
                friendInfo.message_datetime = UnixTimeStampToDateTime(lm.time);
                friendInfo.message_id = lm.status_id;
            }

            if(lc != null && lc.data != null)
            {
                friendInfo.birthday = lc.data[0].birthday;
                friendInfo.location = new Location();
                friendInfo.location.city = lc.data[0].current_location.city == null ? null : lc.data[0].current_location.city.Value;
                friendInfo.location.country = lc.data[0].current_location.country == null ? null : lc.data[0].current_location.country.Value;
                friendInfo.location.latitude = lc.data[0].current_location.latitude == null ? 0 : (decimal)(lc.data[0].current_location.latitude.Value);
                friendInfo.location.longitude = lc.data[0].current_location.longitude == null ? 0 : (decimal)(lc.data[0].current_location.longitude.Value);
                friendInfo.location.state = lc.data[0].current_location.state == null ? null : lc.data[0].current_location.state.Value;
                friendInfo.location.street = lc.data[0].current_location.street == null ? null : lc.data[0].current_location.street.Value;
                friendInfo.location.zip = lc.data[0].current_location.zip == null ? null : lc.data[0].current_location.zip.Value;
            }

            return friendInfo;
        }
 
        #endregion

        #region "Utility Functions"

        public static double GetUnixTime(DateTime DateTimeConvert)
        {
            return (DateTimeConvert - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public static DateTime? UnixTimeStampToDateTime(double unixTimeStamp)
        {
            if (unixTimeStamp == 0) return null;

            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    #endregion
    }
}
