namespace TheFeedbackSystem.API.Migrations
{
    using TheFeedbackSystem.API.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TheFeedbackSystem.API.FeedBackDbContext>
    {
        public Configuration()
        {
        }

        protected override void Seed(TheFeedbackSystem.API.FeedBackDbContext context)
        {
            if (context.SystemClients.Count() > 0)
            {
                return;
            }

            context.SystemClients.AddRange(BuildClientsList());
            context.SaveChanges();
        }

        private static List<SystemClient> BuildClientsList()
        {

            List<SystemClient> ClientsList = new List<SystemClient> 
            {
                new SystemClient
                { Id = "ngAuthApp", 
                    Secret= Helper.GetHash("abc@123"), 
                    Name="AngularJS front-end Application", 
                    ApplicationType =  Models.ApplicationTypes.JavaScript, 
                    Active = true, 
                    RefreshTokenLifeTime = 7200, 
                    AllowedOrigin = "http://localhost:20023/"
                },
                new SystemClient
                { Id = "consoleApp", 
                    Secret=Helper.GetHash("123@abc"), 
                    Name="Console Application", 
                    ApplicationType =Models.ApplicationTypes.NativeConfidential, 
                    Active = true, 
                    RefreshTokenLifeTime = 14400, 
                    AllowedOrigin = "*"
                }
            };

            return ClientsList;
        }
    }
}
