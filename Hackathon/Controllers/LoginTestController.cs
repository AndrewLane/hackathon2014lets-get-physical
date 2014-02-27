using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hackathon.Common;

namespace Hackathon.Controllers
{
    public class LoginTestController : AuthController
    {
        public string Get()
        {
            return UserId + ":" + AuthToken;
        }
    }
}
