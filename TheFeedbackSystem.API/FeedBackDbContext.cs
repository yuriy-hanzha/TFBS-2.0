using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TheFeedbackSystem.API.Models;

namespace TheFeedbackSystem.API
{
    public class FeedBackDbContext : IdentityDbContext<IdentityUser>
    {
        public FeedBackDbContext(): base("FeedBackContext")
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Comment> Feeds { get; set; }
        public DbSet<SystemClient> SystemClients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ConfirmationKey> ConfirmationKeys { get; set; }
    }
}