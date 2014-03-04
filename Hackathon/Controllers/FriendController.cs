using System;
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
                    LastStatusUpdateDateTime = friend.message_datetime,
                    DaysUntilBirthday = GetDaysUntilBirthday(friend.birthday)
                }
                : null;
        }

        private int? GetDaysUntilBirthday(string birthday)
        {
            DateTime parsedDateTime;
            if (DateTime.TryParse(birthday, out parsedDateTime))
            {
                if (parsedDateTime.Month == DateTime.Now.Month && parsedDateTime.Day == DateTime.Now.Day)
                {
                    //today is their birthday
                    return 0;
                }

                var usersNextBirthday = new DateTime(year: DateTime.Now.Year, month: parsedDateTime.Month,
                    day: parsedDateTime.Day);
                if (usersNextBirthday < DateTime.Now.Date)
                {
                    usersNextBirthday = usersNextBirthday.AddYears(1);
                }

                return Convert.ToInt32((usersNextBirthday.Date - DateTime.Now.Date).TotalDays);
            }

            return -1;
        }
    }
}
