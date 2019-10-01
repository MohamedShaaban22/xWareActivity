using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using xWAREActivity.Repository;
using xWAREActivity.ViewModel;

namespace xWAREActivity.Controllers.Web
{
    //[Authorize(Roles = "Admin, User")]
    public class PostWebController : ApiController
    {
        private IPostRepository postRepository;
        private IUserRepository userRepository;
        private ILikeRepository likeRepository;

        public PostWebController()
        {
            this.userRepository = new UserRepository(new ActivityDBEntities());
            this.likeRepository = new LikeRepository(new ActivityDBEntities());
            this.postRepository = new PostRepository(new ActivityDBEntities());
        }

        public PostWebController(IUserRepository userRepository, ILikeRepository likeRepository, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.likeRepository = likeRepository;
            this.postRepository = postRepository;
        }


        [Route("admin/post")]
        [HttpPost]
        public HttpResponseMessage AddPost()
        {
            try
            {
                Post post = new Post();
                string text = HttpContext.Current.Request.Params["text"];
                text = (text != null) ? text.Trim() : null;
                Guid userid = Guid.Parse(HttpContext.Current.Request.Params["userid"]);
                bool filefound = false;
                if (userid != null)
                {
                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        var postedFile = httpRequest.Files[0];
                        if (postedFile.ContentLength > 0)
                        {
                            var fileExt = Path.GetExtension(postedFile.FileName).Substring(1).ToUpper();
                            if (!Configurations.supportedImageTypes.Contains(fileExt) && !Configurations.supportedFileTypes.Contains(fileExt))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Wrong type format");
                            }
                            string extension = '.' + fileExt;
                            string fileName = Path.ChangeExtension(
                                Path.GetRandomFileName(),
                                extension
                            );
                            var filePath = HttpContext.Current.Server.MapPath("~/PostImagesAndFiles/" + fileName);
                            postedFile.SaveAs(filePath);
                            if (Configurations.supportedFileTypes.Contains(fileExt))
                                post.filepath = Configurations.serveruri + "PostImagesAndFiles/" + fileName;
                            else
                                post.image = Configurations.serveruri + "PostImagesAndFiles/" + fileName;
                            filefound = true;
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File or image lenght = 0");
                        }
                    }
                    if (((text != null && filefound == true) || (text != null && filefound == false)) && (text.Length > 0))
                    {
                        post.text = text;
                        post.userid = userid;
                    }
                    else if (text == null && filefound == true)
                    {
                        post.userid = userid;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null Opject");
                    }

                    postRepository.InsertPost(post);
                    postRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, post);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null Opject");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("admin/post")]
        [HttpGet]
        public HttpResponseMessage HomePosts()
        {
            try
            {
                var entity = postRepository.GetPosts();
                PostJsonView posts = new PostJsonView();
                posts.Posts = new List<PostView>();
                posts.Posts = Mapper.Map<List<Post>, List<PostView>>(entity.ToList());
                return Request.CreateResponse(HttpStatusCode.OK, posts);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("admin/user/{id}/post")]
        [HttpGet]
        public HttpResponseMessage UserProfilePosts(Guid id)
        {
            try
            {
                PostJsonView posts = new PostJsonView();
                posts.Posts = new List<PostView>();
                var entity = postRepository.GetUserPosts(id);
                posts.Posts = Mapper.Map<List<Post>, List<PostView>>(entity.ToList());
                return Request.CreateResponse(HttpStatusCode.OK, posts);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("admin/post/{id}")]
        [HttpGet]
        public HttpResponseMessage GetPost(Guid id)
        {
            try
            {
                PostView post = new PostView();
                var entity = postRepository.GetPostByID(id);
                if (entity != null)
                {
                    post = Mapper.Map<Post, PostView>(entity);
                    return Request.CreateResponse(HttpStatusCode.OK, post);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("admin/post/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeletePost(Guid id)
        {
            try
            {
                bool state = postRepository.DeletePost(id);
                if (state)
                {
                    postRepository.DeletePost(id);
                    postRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, "Post Deleted");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found Such a post");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
