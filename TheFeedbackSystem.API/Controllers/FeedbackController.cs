using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using TheFeedbackSystem.API.Models;
using Newtonsoft.Json;

namespace TheFeedbackSystem.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiController
    {
        private FeedBackDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;

        public FeedbackController()
        {
            _dbContext = new FeedBackDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_dbContext));
        }

        [HttpGet]
        [Route("GetInitData")]
        public IHttpActionResult GetData()
        {
            var user = _userManager.Users.FirstOrDefault(i => i.UserName == User.Identity.Name);
            var currentUser = _dbContext.Clients.FirstOrDefault(c => c.UserId == user.Id);
            if (currentUser != null)
            {
                if (DateTime.Today.DayOfYear > currentUser.LastVisit.DayOfYear)
                    currentUser.Likes += (DateTime.Today.DayOfYear - currentUser.LastVisit.DayOfYear) / 3;
                if (currentUser.Likes > 10)
                    currentUser.Likes = 10;

                currentUser.LastVisit = DateTime.Now;
                _dbContext.SaveChanges();
            }

            var feeds = _dbContext.Feeds.ToList();
            var responsedComments = new List<object>();

            foreach (var i in feeds)
            {
                var identityUser = _userManager.Users.FirstOrDefault(u => u.Id == i.AuthorId);
                var author = _dbContext.Clients.FirstOrDefault(a => a.UserId == i.AuthorId);
                responsedComments.Add(new
                {
                    author = identityUser.UserName,
                    text = i.Text,
                    likes = i.Like,
                    dislikes = i.Dislike,
                    avatar = author.Avatar
                });
            }
            object[] obj = new object[] { User.Identity.Name, currentUser.Avatar, responsedComments };
            return Ok(new JavaScriptSerializer().Serialize(obj));
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add([FromBody] JObject rawJsonObject)
        {
            string comm = (string)rawJsonObject["commentText"];
            var identityUser = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var author = _dbContext.Clients.FirstOrDefault(c => c.UserId == identityUser.Id);

            var duplicate = _dbContext.Feeds.FirstOrDefault(d => d.AuthorId == author.UserId && d.Text == comm);
            if (duplicate != null)
                return Ok(Json(new { result = false }));

            _dbContext.Feeds.Add(new Comment
            {
                Id = Guid.NewGuid().ToString(),
                AuthorId = author.UserId,
                Author = author,
                Text = comm,
                Like = 0,
                Dislike = 0
            });
            _dbContext.SaveChanges();
            return Ok(new JavaScriptSerializer().Serialize(new { res = true }));
        }

        [HttpPost]
        [Route("Like")]
        public IHttpActionResult Like(JObject likerData)
        {
            string likerName = (string)likerData["name"],
                likedText = (string)likerData["text"];

            var identityUser = _userManager.Users.FirstOrDefault(i => i.UserName == likerName);
            var client = _dbContext.Clients.FirstOrDefault(c => c.UserId == identityUser.Id && c.Likes > 0);
            var comment = _dbContext.Feeds.FirstOrDefault(t => t.Text == likedText);

            if (client != null && comment != null)
            {
                if (!string.IsNullOrEmpty(comment.DislikedBy) && comment.DislikedBy.Contains(client.UserId))
                    return Ok();

                if (!string.IsNullOrEmpty(comment.LikedBy) && comment.LikedBy.Contains(client.UserId))
                {
                    comment.LikedBy = comment.LikedBy.Replace(identityUser.Id + "; ", "");
                    comment.Like--;
                    client.Likes++;
                    _dbContext.SaveChanges();
                    return Ok(new JavaScriptSerializer().Serialize(new { res = "-" }));
                }
                comment.LikedBy += identityUser.Id + "; ";
                comment.Like++;
                client.Likes--;
                _dbContext.SaveChanges();
                return Ok(new JavaScriptSerializer().Serialize(new { res = "+" }));
            }
            return Ok();
        }

        [HttpPost]
        [Route("Dislike")]
        public IHttpActionResult Dislike(JObject dislikerData)
        {
            string dislikerName = (string)dislikerData["name"],
                dislikedText = (string)dislikerData["text"];

            var identityUser = _userManager.Users.FirstOrDefault(i => i.UserName == dislikerName);
            var client = _dbContext.Clients.FirstOrDefault(c => c.UserId == identityUser.Id && c.Likes > 0);
            var comment = _dbContext.Feeds.FirstOrDefault(t => t.Text == dislikedText);

            if (client != null && comment != null)
            {
                if (!string.IsNullOrEmpty(comment.LikedBy) && comment.LikedBy.Contains(client.UserId))
                    return Ok();

                if (!string.IsNullOrEmpty(comment.DislikedBy) && comment.DislikedBy.Contains(client.UserId))
                {
                    comment.DislikedBy = comment.DislikedBy.Replace(identityUser.Id + "; ", "");
                    comment.Dislike--;
                    client.Likes++;
                    _dbContext.SaveChanges();
                    return Ok(new JavaScriptSerializer().Serialize(new { res = "-" }));
                }
                comment.DislikedBy += identityUser.Id + "; ";
                comment.Dislike++;
                client.Likes--;
                _dbContext.SaveChanges();
                return Ok(new JavaScriptSerializer().Serialize(new { res = "+" }));
            }
            return Ok();
        }
    }
}
