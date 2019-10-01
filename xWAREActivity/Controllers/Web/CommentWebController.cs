using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using xWAREActivity.Interface;
using xWAREActivity.Models;
using xWAREActivity.Repository;
using xWAREActivity.ViewModel;

namespace xWAREActivity.Controllers.Web
{
    //[Authorize(Roles = "Admin, User")]
    public class CommentWebController : ApiController
    {
        private ICommentRepository commentRepository;
        private IPostRepository postRepository;

        public CommentWebController()
        {
            this.commentRepository = new CommentRepository(new ActivityDBEntities());
            this.postRepository = new PostRepository(new ActivityDBEntities());
        }
        public CommentWebController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
        }

        [Route("admin/post/{id}/comment")]
        [HttpGet]
        public HttpResponseMessage PostComments(Guid id)
        {
            try
            {
                var entity = commentRepository.GetPostComments(id);
                CommentJsonView comments = new CommentJsonView();
                comments.comments = new List<CommentView>();
                comments.comments = Mapper.Map<List<Comment>, List<CommentView>>(entity.ToList());
                return Request.CreateResponse(HttpStatusCode.OK, comments);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("admin/post/{postid}/comment/{id}")]
        [HttpGet]
        public HttpResponseMessage CommentById(Guid postid, Guid id)
        {
            try
            {
                var post = postRepository.GetPostByID(postid);
                if (post != null)
                {
                    var entity = commentRepository.GetCommentByID(id);
                    if (entity != null)
                    {
                        CommentView comment = new CommentView();
                        comment = Mapper.Map<Comment, CommentView>(entity);
                        return Request.CreateResponse(HttpStatusCode.OK, comment);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("admin/comment")]
        [HttpPost]
        public HttpResponseMessage AddComment([FromBody] Comment comment)
        {
            try
            {
                if (postRepository.GetPostByID(comment.postid) != null)
                {
                    if (comment != null)
                    {
                        commentRepository.InsertComment(comment);
                        commentRepository.Save();
                        var entity = commentRepository.GetCommentByID(comment.id);
                        CommentView commentview = new CommentView();
                        commentview = Mapper.Map<Comment, CommentView>(entity);
                        return Request.CreateResponse(HttpStatusCode.OK, commentview);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("admin/post/{postid}/comment/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteComment(Guid postid, Guid id)
        {
            try
            {
                if (postRepository.GetPostByID(postid) != null)
                {
                    bool state = commentRepository.DeleteComment(id);
                    commentRepository.Save();
                    if (state)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
                    }
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No such a Comment");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No such a Comment");
            }
            catch (Exception exp)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exp);
            }
        }
    }
}
