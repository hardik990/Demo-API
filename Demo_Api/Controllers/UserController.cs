using Demo_Api.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        public HttpResponseMessage GetUser()
        {
            List<User> lstResult = new List<User>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoAPI"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            User objdata = new User();
                            objdata.Email = rdr["Email"].ToString();
                            objdata.IsActive = bool.Parse(rdr["IsActive"].ToString());
                            objdata.Id = rdr["Id"].ToString();
                            lstResult.Add(objdata);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lstResult);
            }
        }

        [HttpPost, Route("AddUser")]
        public HttpResponseMessage CreateUser(string Email)
        {
            if (!string.IsNullOrEmpty(Email))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoAPI"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                        con.Open();
                        var result = cmd.ExecuteScalar();

                        if (result.ToString() == "0")
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "User Create Successfully.");
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "User Already EXISTS.");
                        }
                    }
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Parameters");
            }

        }
    }
}