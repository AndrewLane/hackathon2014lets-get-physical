using Hackathon.Common;
using Hackathon.Models;

namespace Hackathon.Controllers
{
    public class FriendController : AuthController
    {
        public FriendExtraInfo Get(string id)
        {
            var fbHelper = new FaceBookHelper(AuthToken, UserId);
            var friend = fbHelper.GetFriendExtendedInfo(id);
            return friend != null
                ? new FriendExtraInfo
                {
                    LastStatusUpdate = friend.message,
                    LastStatusId = friend.message_id,
                    LastStatusUpdateDateTime = friend.message_datetime
                }
                : null;
        }
    }
}
