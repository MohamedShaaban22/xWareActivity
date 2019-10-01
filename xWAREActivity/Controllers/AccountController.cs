using AutoMapper;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using xWAREActivity.Repository;
using xWAREActivity.ViewModel;

namespace xWAREActivity.Controllers
{
    public class AccountController : ApiController
    {
        private IUserRepository userRepository;

        public AccountController()
        {
            this.userRepository = new UserRepository(new ActivityDBEntities());
        }

        public AccountController(IUserRepository userRepository, ILikeRepository likeRepository, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login([FromBody]LoginAccount user)
        {

            var resp = new HttpResponseMessage();
            LoginJsonView jsonreturned = new LoginJsonView();
            var item = new JsonTokenLogin();
            try
            {
                var entity = userRepository.FindUser(user.email, user.password);
                if (entity != null)
                {
                    string encodedUserCredentials =
                            Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("user:password"));
                    string userData = "username=" + user.email + "&password=" + user.password + "&grant_type=password";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Configurations.APITokenURL);
                    request.Accept = "application/json";
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Headers.Add("Authorization", "Bearer " + encodedUserCredentials);

                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                    requestWriter.Write(userData);
                    requestWriter.Close();

                    var response = request.GetResponse() as HttpWebResponse;

                    string jsonString;
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        jsonString = reader.ReadToEnd();
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    jsonreturned.token = new JsonTokenLogin();
                    jsonreturned.token = js.Deserialize<JsonTokenLogin>(jsonString);

                    jsonreturned.user = new UserView();
                    jsonreturned.user = Mapper.Map<User, UserView>(entity);

                    resp = Request.CreateResponse(HttpStatusCode.OK, jsonreturned);
                }
                else
                    resp = Request.CreateErrorResponse(HttpStatusCode.NotFound, "The user Not found");

                return resp;
            }
            catch (Exception exp)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exp);
            }
        }
    }
}
