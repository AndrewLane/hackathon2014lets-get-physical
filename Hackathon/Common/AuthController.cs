using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Hackathon.Common
{
    public abstract class AuthController : ApiController
    {
        private AuthToken authTokenCache = null;
        public bool IsUserLoggedIn
        {
            get
            {
                return string.IsNullOrEmpty(AuthToken);
            }
        }
        public string AuthToken
        {
            get
            {
                if (authTokenCache == null) authTokenCache = new AuthToken(HttpContext.Current.Request);
                return authTokenCache.Token ?? "";
            }
        }

        public string UserId
        {
            get
            {
                if (authTokenCache == null) authTokenCache = new AuthToken(HttpContext.Current.Request);
                return authTokenCache.UserId ?? "";
            }
        }

    }
}