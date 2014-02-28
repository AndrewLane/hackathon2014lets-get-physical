using System.Linq;
using Hackathon.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Hackathon.Common;
using Microsoft.Ajax.Utilities;

namespace Hackathon.Controllers
{
    public class FriendsController : AuthController
    {
        // GET api/friends
        public IEnumerable<FriendInfoCard> Get()
        {
            var fbHelper = new FaceBookHelper(AuthToken, UserId);

            return
                fbHelper.GetFriends().Select(friend => new FriendInfoCard { FriendId = friend.uid, FullName = friend.name });
        }

        // GET api/friends/5
        public FriendInfoCard Get(int id)
        {
            var fbHelper = new FaceBookHelper(AuthToken, UserId);

            var allFriends =
                fbHelper.GetRankedFriends()
                    .Select(
                        friend =>
                            new FriendInfoCard
                            {
                                FriendId = friend.uid,
                                FullName = friend.name,
                                VirtualInteractionScore = friend.virtualRank,
                                PhysicalInteractionScore = friend.physicalRank,
                                LastSocialUpdateMetadata = "Loading...",
                                ProfilePictureImagePath = friend.pic_small
                            })
                    .OrderByDescending(friend => friend.TotalInteractionScore)
                    .ToList();

            if (allFriends.Any() == false)
            {
                return null;
            }

            id = id % allFriends.Count();
            if (id < 1)
            {
                id = allFriends.Count();
            }
            return allFriends[id - 1];
        }

        // POST api/friends
        public void Post([FromBody]string value)
        {
        }

        // PUT api/friends/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/friends/5
        public void Delete(int id)
        {
        }
    }
}
