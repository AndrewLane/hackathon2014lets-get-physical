using Hackathon.Common;
using Hackathon.Models;
using System.Linq;

namespace Hackathon.Controllers
{
    public class FriendsController : AuthController
    {
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
                                ProfilePictureImagePath = friend.pic_big,
                                LastInteractionTime = friend.lastInteractionTime
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
            var friendToReturn = allFriends[id - 1];
            friendToReturn.FriendRank = id;
            return friendToReturn;
        }
    }
}
