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

namespace xWAREActivity.Controllers.Mobile
{
   // [Authorize(Roles = "Admin, User")]
    public class LikeMobileController : ApiController
    {
        private IPostRepository postRepository;
        private IUserRepository userRepository;
        private ILikeRepository likeRepository;

        public LikeMobileController()
        {
            this.userRepository = new UserRepository(new ActivityDBEntities());
            this.likeRepository = new LikeRepository(new ActivityDBEntities());
            this.postRepository = new PostRepository(new ActivityDBEntities());
        }

        public LikeMobileController(IUserRepository userRepository, ILikeRepository likeRepository, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.likeRepository = likeRepository;
            this.postRepository = postRepository;
        }

        [Route("api/like")]
        [HttpPost]
        public HttpResponseMessage AddLike([FromBody]Like like)
        {
            try {
                var post = postRepository.GetPostByID(like.postid);
                if (post != null && post.userid != like.userid && userRepository.GetUserById(like.userid) != null)
                {
                    likeRepository.InsertLike(like);
                    likeRepository.Save();
                    LikeView likeview = new LikeView();
                    likeview = Mapper.Map<Like, LikeView>(like);
                    return Request.CreateResponse(HttpStatusCode.OK, likeview);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong Entered Format");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("api/post/{id}/like")]
        [HttpGet]
        public HttpResponseMessage PostLikes(Guid id)
        {
            try
            {
                var likes = likeRepository.PostLikes(id);
                LikeView likeview = new LikeView();
                LikeJsonView Likes = new LikeJsonView();
                Likes.likes = new List<LikeView>();
                Likes.likes = Mapper.Map<List<Like>,List< LikeView>>(likes.ToList());
                return Request.CreateResponse(HttpStatusCode.OK, Likes);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //make trigger to decreament counterlike
        [Route("api/post/{id}/like/{likeid}")]
        [HttpDelete]
        public HttpResponseMessage DeleteLike(Guid id,Guid likeid)
        {
            try
            {
                if (postRepository.GetPostByID(id) != null)
                {
                    bool like = likeRepository.DeleteLike(likeid);
                    if (like)
                        return Request.CreateResponse(HttpStatusCode.OK);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Not Liked");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No such a post");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
