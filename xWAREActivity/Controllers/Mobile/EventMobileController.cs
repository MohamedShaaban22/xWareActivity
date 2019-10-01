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

namespace xWAREActivity.Controllers.Mobile
{
    //[Authorize(Roles = "Admin, User")]
    public class EventMobileController : ApiController
    {
        private IEventRepository EventRepository;
        

        public EventMobileController()
        {
            this.EventRepository = new EventRepository(new ActivityDBEntities());
        }
        public EventMobileController(IEventRepository EventRepository)
        {
            this.EventRepository = EventRepository;
        }

        [Route("api/event")]
        [HttpPost]
        public HttpResponseMessage AddEvent([FromBody]Event _event)
        {
            try
            {
                if (_event != null)
                {
                    EventRepository.InsertEvent(_event);
                    EventDBView entity = new EventDBView();
                    entity._event = _event;
                    EventRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception exp)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exp);
            }
        }

        [Route("api/event/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(Guid id)
        {
            try
            {
                bool state = EventRepository.DeleteEvent(id);
                EventRepository.Save();
                if (state)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,"Deleted");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No such a user");
            }
            catch (Exception exp)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exp);
            }
        }

        [Route("api/event")]
        [HttpPut]
        public HttpResponseMessage EditEvent([FromBody]Event _event)
        {
            try
            {
                
                if (_event != null)
                {
                    EventRepository.UpdateEvent(_event);
                    EventRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK,_event);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception exp)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exp);
            }
        }

        [Route("api/event/{id}")]
        [HttpGet]
        public HttpResponseMessage EventDetails(Guid id)
        {
            try
            {
                var entity = EventRepository.GetEventByID(id);
                if (entity != null)
                {
                    EventDescriptionView eventview = new EventDescriptionView();
                    eventview.title = entity.title;
                    eventview.team = (entity.User.Team != null) ? entity.User.Team.name : "Individual User";
                    eventview.start = entity.start;
                    eventview.end = entity.finish;
                    eventview.description = entity.description;
                    eventview.date = entity.date;
                    eventview.eventid = entity.id;
                    eventview.userid = entity.userid;

                    EventDiscview entit = new EventDiscview();
                    entit._event = eventview;
                    return Request.CreateResponse(HttpStatusCode.OK, entit);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception exp)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exp);
            }
        }

        [Route("api/event")]
        [HttpGet]
        public HttpResponseMessage EventsView()
        {
            try
            {
                var entity = EventRepository.GetEvents();
                if (entity != null)
                {
                    Events events = new Events();
                    events.events = new List<EventView>();
                    EventView ent;
                    foreach (var _event in entity)
                    {
                        ent = new EventView();
                        ent.eventid = _event.id;
                        ent.date = _event.date;
                        ent.start = _event.start;
                        ent.teamname = (_event.User.Team!=null)? _event.User.Team.name:"Individual User";
                        ent.title = _event.title;
                        events.events.Add(ent);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, events);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception exp)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exp);
            }
        }
    }
}
