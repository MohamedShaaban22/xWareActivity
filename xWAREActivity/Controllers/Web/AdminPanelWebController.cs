
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
    //[Authorize(Roles = "Admin")]
    public class AdminPanelWebController : ApiController
    {
        private IUserRepository userRepository;
        private ILikeRepository likeRepository;
        private ITeamRepository teamRepository;


        public AdminPanelWebController()
        {
            this.userRepository = new UserRepository(new ActivityDBEntities());
            this.likeRepository = new LikeRepository(new ActivityDBEntities());
            this.teamRepository = new TeamRepository(new ActivityDBEntities());


        }

        public AdminPanelWebController(IUserRepository userRepository, ILikeRepository likeRepository, ITeamRepository teamRepository)
        {
            this.userRepository = userRepository;
            this.likeRepository = likeRepository;
            this.teamRepository = teamRepository;

        }


        //---------------------------------USER---------------------------------------------//
        [Route("adminpanel/user")]
        [HttpGet]
        public HttpResponseMessage users()
        {
            try
            {
                var entity = userRepository.GetUsers();
                List<AdminUserView> users = new List<AdminUserView>();

                users = Mapper.Map<List<User>, List<AdminUserView>>(entity.ToList());
                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("adminpanel/user")]
        [HttpPost]
        public HttpResponseMessage Adduser([FromBody]User user)
        {
            try
            {
                if (user != null)
                {
                    userRepository.InsertUser(user);
                    userRepository.Save();
                    var entity = userRepository.GetUserById(user.id);
                    AdminUserView userview = new AdminUserView();
                    userview = Mapper.Map<User, AdminUserView>(entity);
                    userRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, userview);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null object");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("adminpanel/user/{id}/password/reset")]
        [HttpPost]
        public HttpResponseMessage ResetPassword(Guid id,[FromBody]string password)
        {
            try
            {
                var entity = userRepository.GetUserById(id);
                if (entity != null&&(password!=null || password.Trim()!=""))
                {
                    entity.password = password;
                    userRepository.UpdateUser(entity);
                    userRepository.Save();
                    AdminUserView userview = new AdminUserView();
                    userview = Mapper.Map<User, AdminUserView>(entity);
                    userRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, userview);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null object");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("adminpanel/user/{id}")]
        [HttpDelete]
        public HttpResponseMessage Deleteuser(Guid id)
        {
            try
            {
                var status = userRepository.DeleteUser(id);
                if (status)
                {
                    userRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("adminpanel/user")]
        [HttpPut]
        public HttpResponseMessage Edituser([FromBody]AdminUserView user)
        {
            try
            {
                if (user != null)
                {
                    var entity = userRepository.GetUserById(user.id);
                    if (entity != null)
                    {
                        entity.email = user.email;
                        entity.name = user.name;
                        entity.Role = user.role;
                        entity.teamid = user.team.id;
                        userRepository.UpdateUser(entity);
                        userRepository.Save();
                        entity = userRepository.GetUserById(user.id);
                        AdminUserView userview = new AdminUserView();
                        userview = Mapper.Map<User, AdminUserView>(entity);                        
                        return Request.CreateResponse(HttpStatusCode.OK, userview);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Found");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null object");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //---------------------------------TEAM---------------------------------------------//
        [Route("adminpanel/team")]
        [HttpGet]
        public HttpResponseMessage Teams()
        {
            try
            {
                var entity = teamRepository.GetTeams();
                TeamJsonView teams = new TeamJsonView();
                teams.Teams = new List<AdminTeamView>();
                teams.Teams = Mapper.Map<List<Team>, List<AdminTeamView>>(entity.ToList());
                return Request.CreateResponse(HttpStatusCode.OK, teams);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("adminpanel/team")]
        [HttpPost]
        public HttpResponseMessage AddTeam(Team team)
        {
            try
            {
                if (team != null)
                {
                    teamRepository.InsertTeam(team);
                    teamRepository.Save();
                    AdminTeamView teamview = new AdminTeamView();
                    teamview = Mapper.Map<Team, AdminTeamView>(team);
                    return Request.CreateResponse(HttpStatusCode.OK, teamview);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null Object");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("adminpanel/team")]
        [HttpPut]
        public HttpResponseMessage EditTeam(Team team)
        {
            try
            {
                if (team != null)
                {
                    teamRepository.UpdateTeam(team);
                    teamRepository.Save();
                    AdminTeamView teamview = new AdminTeamView();
                    teamview = Mapper.Map<Team, AdminTeamView>(team);
                    return Request.CreateResponse(HttpStatusCode.OK, teamview);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null Object");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("adminpanel/team/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteTeam(Guid id)
        {
            try
            {
                var status = teamRepository.DeleteTeam(id);
                if (status)
                {    
                    teamRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
