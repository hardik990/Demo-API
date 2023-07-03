using Demo_Api.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Demo_Api.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            Demo_Api.Models.Login _Login = new Demo_Api.Models.Login();
            if (Session["ErrorMessage"] != null && Session["ErrorMessage"].ToString() != string.Empty)
            {
                _Login.ErrorMessage = Session["ErrorMessage"].ToString();
            }
            else
            {
                _Login.ErrorMessage = string.Empty;
            }

            return this.View(_Login);
        }

        public ActionResult LoginwithAD()
        {
            try
            {

                var state = Guid.NewGuid().ToString();
                Response.Cookies.Add(new HttpCookie("myo365auth", state)
                {
                    Secure = true,
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMinutes(2)
                });

                string url = String.Format("https://login.microsoftonline.com/{0}/oauth2/authorize" +
                                           "?client_id={1}&redirect_uri={2}&response_type=code&state={2}",
                                            ConfigurationManager.AppSettings["ida:TenantId"],
                                            ConfigurationManager.AppSettings["ida:ClientId"],
                                            ConfigurationManager.AppSettings["ida:RedirectUri"],
                                            state);

                return Redirect(url);
            }
            catch (Exception Ex)
            {

                return this.View();
            }
        }


        public ActionResult Logingamesglobal()
        {
            try
            {

                if (Request.QueryString["error"] != null)
                {
                    throw new HttpException((int)HttpStatusCode.Forbidden,
                                            "Error while authenticating your Office365 account",
                                            new Exception("Office365 Error (1:auth): " + Request.QueryString["error_description"]));
                }
                var code = Request.QueryString["code"];
                var url = String.Format("https://login.microsoftonline.com/{0}/oauth2/token", ConfigurationManager.AppSettings["ida:TenantId"]);
                var content = String.Format("client_id={0}&code={1}&grant_type=authorization_code&redirect_uri={2}" +
                                            "&resource={3}&client_secret={4}",
                                             ConfigurationManager.AppSettings["ida:ClientId"],
                                            code,
                                             ConfigurationManager.AppSettings["ida:RedirectUri"],
                                            "https://graph.microsoft.com",
                                            HttpUtility.UrlEncode(ConfigurationManager.AppSettings["ida:ClientSecret"]));


                var webRequest = HttpWebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentLength = content.Length;

                using (var reqStream = webRequest.GetRequestStream())
                {
                    using (var writer = new StreamWriter(reqStream))
                    {
                        writer.Write(content);
                    }
                }

                string tokenResponse = "";
                var webResponse = webRequest.GetResponse();
                using (var respStream = webResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(respStream))
                    {
                        tokenResponse = reader.ReadToEnd();
                    }
                }
                var response = JsonConvert.DeserializeObject<Office365TokenResponse>(tokenResponse);
                if (response != null && response.AccessToken != null && response.AccessToken != string.Empty)
                {
                    string baseUrl = $"https://graph.microsoft.com/v1.0/me/";
                    HttpClient _azure = new HttpClient();
                    _azure.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.AccessToken);
                    HttpResponseMessage result = _azure.GetAsync(baseUrl).Result;
                    result.EnsureSuccessStatusCode();
                    var email = JObject.Parse(result.Content.ReadAsStringAsync().Result)["mail"];
                    if (email != null && email.ToString() != string.Empty)
                    {
                        return this.Redirect("https://localhost:44342/swagger");

                    }
                    else
                    {
                        Session["ErrorMessage"] = "Please contact your administrator,Your login request sent for authorization";
                        return this.RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    Session["ErrorMessage"] = "Login Invalid";
                    return this.RedirectToAction("Index", "Home");
                }
            }
            catch (Exception Ex)
            {
                Session["ErrorMessage"] = "Please contact your administrator,access required";
                return this.RedirectToAction("Index", "Home");
            }
        }
    }
}