using Demo_Api.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Demo_Api.Controllers
{
    [RoutePrefix("api/Shopping")]
    public class ShoppingController : ApiController
    {
        [HttpGet, Route("ShoppingList")]
        public HttpResponseMessage GetShoppingList(string email)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost, Route("AddItem")]
        public HttpResponseMessage AddItem([FromBody] ShoppingItem objItem)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost, Route("uploadImage")]
        public HttpResponseMessage uploadImage([FromBody] ShoppingItem objItem)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPut, Route("UpdateItem")]
        public HttpResponseMessage UpdateItem([FromBody] ShoppingItem objItem)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpDelete, Route("DeleteItem")]
        public HttpResponseMessage DeleteItem(string ItemId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }


    }
}