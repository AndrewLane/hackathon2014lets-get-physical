using System;

namespace Hackathon.Models
{
    public class CheckIn
    {
        public string id;
        public DateTime created_time;
        public Friend from;
        public string type;
        public Place place;
    }
}