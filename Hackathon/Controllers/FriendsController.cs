using System.Linq;
using Hackathon.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Hackathon.Common;

namespace Hackathon.Controllers
{
    public class FriendsController : AuthController
    {
        // GET api/friends
        public IEnumerable<FriendInfoCard> Get()
        {
            var fbHelper = new FaceBookHelper(AuthToken, UserId);

            return
                fbHelper.GetFriends().Select(friend => new FriendInfoCard { FriendId = friend.id, FullName = friend.name });
        }

        FriendInfoCard GetDummyCard(int index)
        {
            var physicalScore = 10 - index;
            if (physicalScore < 0) physicalScore = 0;

            var virtualScore = index;
            if (virtualScore > 10) virtualScore = 10;

            return new FriendInfoCard
            {
                FriendId = String.Format("friendwithindex{0}", index),
                FullName = String.Format("Friend #{0}", index),
                LastSocialUpdateMetadata = "Status update from this person",
                PhysicalInteractionScore = physicalScore,
                VirtualInteractionScore = virtualScore,
                ProfilePictureImagePath = "https://pbs.twimg.com/profile_images/2619899212/2zch7tkkmu327fkdaobt.jpeg"
            };
        }

        // GET api/friends/5
        public FriendInfoCard Get(int id)
        {
            return GetDummyCard(id);
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
