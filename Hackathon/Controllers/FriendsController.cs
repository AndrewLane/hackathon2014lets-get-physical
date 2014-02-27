using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hackathon.Models;

namespace Hackathon.Controllers
{
    public class FriendsController : ApiController
    {

        private FriendInfoCard MockFriend()
        {
            var x = new FriendInfoCard();
            x.FullName = "Dan Kang";
            x.FriendId = "dankang";
            x.LastSocialUpdateMetadata = "Hackathon friend";
            x.PhysicalInteractionScore = 10;
            x.VirtualInteractionScore = 5;
            return x;
        }
        // GET api/friends
        public IEnumerable<FriendInfoCard> Get()
        {
            return new List<FriendInfoCard>() {
                MockFriend(), MockFriend(), MockFriend(), MockFriend(), MockFriend()
            };
        }

        // GET api/friends/5
        public FriendInfoCard Get(int id)
        {
            return MockFriend();
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
