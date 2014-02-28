namespace Hackathon.Models
{
    /// <summary>
    /// Models the information shows on the friend leaderboard screen
    /// </summary>
    public class FriendInfoCard
    {
        /// <summary>
        /// Identifer for this friend in FB
        /// </summary>
        public string FriendId { get; set; }

        /// <summary>
        /// Path to an image that can be used to produce an <img /> tag for your friend's profile picture
        /// </summary>
        public string ProfilePictureImagePath { get; set; }

        /// <summary>
        /// Full name of your friend
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Metadata about the last social update (i.e. status update, picture posted) that this friend made
        /// </summary>
        public string LastSocialUpdateMetadata { get; set; }

        /// <summary>
        /// Numerical "physical" interaction score with this friend
        /// </summary>
        public int PhysicalInteractionScore { get; set; }

        /// <summary>
        /// Numerical "virtual" interaction score with this friend
        /// </summary>
        public int VirtualInteractionScore { get; set; }

        /// <summary>
        /// Sum of virtual and physical interaction scores
        /// </summary>
        public int TotalInteractionScore { get { return PhysicalInteractionScore + VirtualInteractionScore; } }
    }
}