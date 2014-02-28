using System;

namespace Hackathon.Models
{
    public class RankedFriend : Friend
    {
        public int physicalRank;
        public int virtualRank;
        public DateTime? lastInteractionTime;

        public RankedFriend(Friend friend, int VirtualRank, int PhysicalRank, DateTime? LastInteractionTime)
        {
            uid = friend.uid;
            name = friend.name;
            pic_big = friend.pic_big;
            physicalRank = PhysicalRank;
            virtualRank = VirtualRank;
            lastInteractionTime = LastInteractionTime;
        }
    }
}