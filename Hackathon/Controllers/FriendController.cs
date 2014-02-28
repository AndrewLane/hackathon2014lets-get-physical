using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hackathon.Models;
using Hackathon.Common;

namespace Hackathon.Controllers
{
    public class FriendController : AuthController
    {
        public FriendInfoCard Get(string id)
        {
            var fbHelper = new FaceBookHelper(AuthToken, UserId);
            var friend = fbHelper.GetFriend(id);
            if (friend != null)
            {
                return new FriendInfoCard { FriendId = friend.uid, FullName = friend.name };
            }
            else
            {
                return null;
            }
        }
    }
}
