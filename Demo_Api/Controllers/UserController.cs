using Demo_Api.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Demo_Api.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet, Route("GetUser")]
        public HttpResponseMessage GetUser(string Email)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost, Route("AddUser")]
        public HttpResponseMessage CreateUser([FromBody] User objUser)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }
    }
}