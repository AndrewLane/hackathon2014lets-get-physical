using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Common
{
    public class AuthToken
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public AuthToken(HttpRequest request)
        {
            IEnumerable<string> tokenHeaders = request.Headers.GetValues("FacebookAuthToken");
            IEnumerable<string> userHeaders = request.Headers.GetValues("FacebookUserId");
            if (tokenHeaders != null) Token = tokenHeaders.FirstOrDefault();
            if (userHeaders != null) UserId = userHeaders.FirstOrDefault();
        }
    }
}