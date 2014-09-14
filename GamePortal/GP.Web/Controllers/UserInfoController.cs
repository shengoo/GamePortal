using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GP.Web.DAL;

namespace GP.Web.Controllers
{
    public class UserInfoController : ApiController
    {
        private gamedb db = new gamedb();

        // GET api/UserInfo
        public IEnumerable<userinfo> Getuserinfoes()
        {
            return db.userinfo.AsEnumerable();
        }

        // GET api/UserInfo/5
        public userinfo Getuserinfo(long id)
        {
            userinfo userinfo = db.userinfo.Find(id);
            if (userinfo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return userinfo;
        }

        // PUT api/UserInfo/5
        public HttpResponseMessage Putuserinfo(long id, userinfo userinfo)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != userinfo.id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(userinfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/UserInfo
        public HttpResponseMessage Postuserinfo(userinfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.userinfo.Add(userinfo);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, userinfo);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = userinfo.id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/UserInfo/5
        public HttpResponseMessage Deleteuserinfo(long id)
        {
            userinfo userinfo = db.userinfo.Find(id);
            if (userinfo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.userinfo.Remove(userinfo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, userinfo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}