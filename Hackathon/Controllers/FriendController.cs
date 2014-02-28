using Hackathon.Common;
using Hackathon.Models;

namespace Hackathon.Controllers
{
    public class FriendController : AuthController
    {
        public FriendExtraInfo Get(string id)
        {
            var fbHelper = new FaceBookHelper(AuthToken, UserId);
            var friend = fbHelper.GetFriend(id);
            return friend != null ? new FriendExtraInfo {LastStatusUpdate = "last status update goes here"} : null;
        }
    }
}
