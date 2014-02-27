﻿using System.Linq;
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
                fbHelper.GetFriends().Select(friend => new FriendInfoCard { FriendId = friend.id, FullName = friend.name });
        }

        // GET api/friends/5
        public FriendInfoCard Get(int id)
        {
            var fbHelper = new FaceBookHelper(AuthToken, UserId);

            var allFriends =
                fbHelper.GetFriends().Select(friend => new FriendInfoCard { FriendId = friend.id, FullName = friend.name }).ToList();

            if (allFriends.Any() == false)
            {
                return null;
            }

            if (id > allFriends.Count())
            {
                id = allFriends.Count();
            }
            if (id < allFriends.Count())
            {
                id = 1;
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
