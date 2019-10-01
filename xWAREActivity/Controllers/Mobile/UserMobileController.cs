using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using xWAREActivity.Repository;
using xWAREActivity.ViewModel;
using System.Web;
using System.IO;

namespace xWAREActivity.Controllers.Mobile
{
   // [Authorize(Roles = "Admin, User")]
    public class UserMobileController : ApiController
    {
        private IUserRepository userRepository;
        private ILikeRepository likeRepository;
        private IPostRepository postRepository;

        public UserMobileController()
        {
            this.userRepository = new UserRepository(new ActivityDBEntities());
            this.likeRepository = new LikeRepository(new ActivityDBEntities());
            this.postRepository = new PostRepository(new ActivityDBEntities());
        }

        public UserMobileController(IUserRepository userRepository, ILikeRepository likeRepository,IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.likeRepository = likeRepository;
            this.postRepository = postRepository;
        }

        [Route("api/user/{id}")]
        [HttpGet]
        public HttpResponseMessage Userinformation(Guid id)
        {
            ProfileView profile = new ProfileView();
            var entity = userRepository.GetUserById(id);
            if (entity != null)
            {
                profile.email = entity.email;
                profile.image = entity.image;
                profile.name = entity.name;
                profile.userid = entity.id;
                profile.totallikes = likeRepository.UserTotalLikes(id);
                profile.team = (entity.Team==null)?"Not Joined a Team":entity.Team.name;
                profile.rank = userRepository.UserRank(id);
                return Request.CreateResponse(HttpStatusCode.OK, profile);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Didn't find a user with this id");
        }

        [Route("api/rank")]
        [HttpGet]
        public HttpResponseMessage TotalRank()
        {
            try
            {
                rank rank = new rank();
                rank.ranks = userRepository.TotalRank().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, rank);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("api/user/{id}")]
        [HttpPost]
        public HttpResponseMessage EditUser(Guid id)
        {
            var entity = userRepository.GetUserById(id);

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            string name = HttpContext.Current.Request.Params["name"];
            bool filefound = (httpRequest.Files.Count > 0) ? true : false;

            if (name == null && filefound==false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else if (filefound || name != null)
            {
                if (name != null)
                {
                    entity.name = name;
                }
                if (filefound)
                {
                    var postedFile = httpRequest.Files[0];
                    if (postedFile.ContentLength > 0)
                    {
                        var fileExt = Path.GetExtension(postedFile.FileName).Substring(1).ToUpper();
                        if (!Configurations.supportedImageTypes.Contains(fileExt))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Wrong type format");
                        }
                        string extension = '.' + fileExt;
                        string fileName = Path.ChangeExtension(
                            Path.GetRandomFileName(),
                            extension
                        );
                        var filePath = HttpContext.Current.Server.MapPath("~/Images/" + fileName);
                        postedFile.SaveAs(filePath);
                        entity.image = Configurations.serveruri + "Images/" + fileName;

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }

                userRepository.UpdateUser(entity);
                userRepository.Save();

                ProfileView profile = new ProfileView();

                profile.email = entity.email;
                profile.image = entity.image;
                profile.name = entity.name;
                profile.userid = entity.id;
                profile.totallikes = likeRepository.UserTotalLikes(id);
                profile.team = (entity.Team == null) ? "Not Joined a Team" : entity.Team.name;
                profile.rank = userRepository.UserRank(id);

                result = Request.CreateResponse(HttpStatusCode.OK, profile);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }

        [Route("api/user/{id}")]
        [HttpPut]
        public HttpResponseMessage EditPassword(Guid id, [FromBody]PassowrdChange password)
        {
            try
            {
                var user = userRepository.GetUserById(id);
                HttpResponseMessage result = null;
                if (user != null && password.oldpassword != null && password.newpassword != null)
                {
                    if (password.oldpassword != password.newpassword)
                    {
                        if (user.password == password.oldpassword)
                        {
                            user.password = password.newpassword;
                            userRepository.UpdateUser(user);
                            userRepository.Save();
                            ProfileView profile = new ProfileView();
                            var entity = userRepository.GetUserById(id);
                            profile.email = entity.email;
                            profile.image = entity.image;
                            profile.name = entity.name;
                            profile.userid = entity.id;
                            profile.totallikes = likeRepository.UserTotalLikes(id);
                            profile.team = (entity.Team == null) ? "Not Joined a Team" : entity.Team.name;
                            profile.rank = userRepository.UserRank(id);
                            result = Request.CreateResponse(HttpStatusCode.OK, profile);
                        }
                        else
                            result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong old password");
                    }
                    else
                        result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Old Passowrd equals new password");
                }
                else
                    result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Nul opjects");
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
