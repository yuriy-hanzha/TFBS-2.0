using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace TheFeedbackSystem.API.Controllers
{
    [RoutePrefix("api/confirmation")]
    public class ConfirmationController : ApiController
    {
        private FeedBackDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;

        public ConfirmationController()
        {
            _dbContext = new FeedBackDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_dbContext));
        }

        [HttpPost]
        [Route("Check")]
        public IHttpActionResult CheckConfirmId(JObject confirmId)
        {
            var keysList = _dbContext.ConfirmationKeys.ToList();
            foreach (var keyObj in keysList)
            {
                if (keyObj.Key == (string)confirmId["id"])
                {
                    var identityUser = _userManager.Users.FirstOrDefault(u => u.Id == keyObj.UserId);
                    identityUser.EmailConfirmed = true;
                    _dbContext.ConfirmationKeys.Remove(keyObj);
                    _dbContext.SaveChanges();
                    return Ok(new JavaScriptSerializer().Serialize(new { name = identityUser.UserName }));
                }
            }
            return BadRequest();
        }
    }
}
