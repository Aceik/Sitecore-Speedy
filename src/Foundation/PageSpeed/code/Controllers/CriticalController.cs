using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Foundation.PageSpeed.Controllers
{
    using Site.Foundation.PageSpeed.Events;
    using Site.Foundation.PageSpeed.Model;
    using Sitecore.Data.Items;
    using Sitecore.SecurityModel;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Cors;
    
    public class CriticalController : ApiController
    {
        public CriticalController()
        {

        }

        [HttpGet]
        public IHttpActionResult Generate()
        {
            var resp = new HttpResponseMessage
            {
                Content = new StringContent("Some Content", Encoding.UTF8, "text/xml")
            };

            // Your code here

            return ResponseMessage(resp);
        }

        [AllowAnonymous]
        [HttpGet]
        [System.Web.Http.Route("critical/api/get/{id}")]
        public HttpResponseMessage Get([FromUri]string id)
        {
            Item item = Sitecore.Data.Database.GetDatabase("master").GetItem(new Sitecore.Data.ID(id));
            string url = SpeedyPageOnSaveEvent.GetUrlForContextSite(item) + $"?{SpeedyConstants.ByPass.ByPassParameter}=true";
            return this.Request.CreateResponse(HttpStatusCode.OK, url);
        }

        [AllowAnonymous]
        [HttpPost]
        [System.Web.Http.Route("critical/api/put")]
        public HttpResponseMessage Put([FromBody] CriticalJson submit)
        {
            Item item = Sitecore.Data.Database.GetDatabase("master").GetItem(new Sitecore.Data.ID(submit.Id));
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item.Fields[SpeedyConstants.Fields.CriticalCSS].Value = submit.Result;
                item.Editing.EndEdit();
                item.Editing.AcceptChanges();
            }

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}