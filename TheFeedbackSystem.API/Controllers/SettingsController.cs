using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json.Linq;

namespace TheFeedbackSystem.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/settings")]
    public class SettingsController : ApiController
    {
        private FeedBackDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;

        public SettingsController()
        {
            _dbContext = new FeedBackDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_dbContext));
        }

        [HttpPost]
        [Route("SaveImage")]
        public IHttpActionResult SaveImage(JObject newAvatarObj)
        {
            var identityUser = _userManager.FindByName(User.Identity.Name);
            var client = _dbContext.Clients.FirstOrDefault(c => c.UserId == identityUser.Id);
            client.Avatar = (string) newAvatarObj["img"];
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}