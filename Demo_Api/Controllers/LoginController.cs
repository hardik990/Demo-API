using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Demo_Api.Controllers
{
    [RoutePrefix("api/Session")]
    public class LoginController : ApiController
    {
        [AllowAnonymous, HttpPost, Route("login")]
        public HttpResponseMessage login()
        {

            return Request.CreateResponse(HttpStatusCode.OK, "");
        }
    }
}