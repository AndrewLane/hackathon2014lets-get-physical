using System;

namespace Hackathon.Models
{
    /// <summary>
    /// Models extra information that's pulled for a friend
    /// </summary>
    public class FriendExtraInfo
    {
        /// <summary>
        /// The last status update that this user made
        /// </summary>
        public string LastStatusUpdate { get; set; }

        /// <summary>
        /// If applicable, the time of the last status update
        /// </summary>
        public DateTime? LastStatusUpdateDateTime { get; set; }

        /// <summary>
        /// The ID of the last status update
        /// </summary>
        public string LastStatusId { get; set; }

        /// <summary>
        /// Days until this friend's next birthday
        /// </summary>
        public int? DaysUntilBirthday { get; set; }
    }
}