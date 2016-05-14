using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Microsoft.AspNet.Identity;
using TheFeedbackSystem.API.Models;

namespace TheFeedbackSystem.API
{
    public class MailService
    {
        private FeedBackDbContext _ctx;
        private UserManager<IdentityUser> _userManager;
        private SmtpClient smtpServer;
        
        public MailService()
        {
            _ctx = new FeedBackDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            smtpServer = new SmtpClient(" ")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(" ", " "),
                EnableSsl = false
            };
        }

        public void SendWelcomeMessage(IdentityUser user)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress("thefeedbacksystem@radacode.net"),
                To = { user.Email },
                Subject = "Welcome | TFBS",
                Body = $"Hello, {user.UserName} <br/>" +
                       $"Your account has been created - now you can send feedback about our service. <br/>" +
                       $"<a href=\"http://localhost:27680/#/\">" +
                       $"   <button>Get Started</button>" +
                       $"</a><br/>" +
                       $"Thank you for joining us! <br/>" +
                       $"TFBS team",
                IsBodyHtml = true,
            };

            smtpServer.Send(mail);
        }

        public void SendConfirmationMessage(IdentityUser user)
        {
            string userId = _userManager.FindByName(user.UserName).Id;
            string key = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            _ctx.ConfirmationKeys.Add(new ConfirmationKey()
            {
                UserId = userId,
                Key = key
            });
            _ctx.SaveChanges();

            MailMessage mail = new MailMessage()
            {
                From = new MailAddress("thefeedbacksystem@radacode.net"),
                To = { user.Email },
                Subject = "The last stage of registration | TFBS",
                Body = $"<h3>Do not forget to confirm your TFBS account</h3>" +
                       $"<h4>Hello, {user.UserName}! </br>" +
                       $"You recently created an account on TFBS. Confirm it in order to complete the registration. <br/>" +
                       $"<a href=\"http://localhost:27680/#/confirmation/{key}\">" +
                       $"   <button>Verify your account</button>" +
                       $"</a><br/>" +
                       $"Thank you for joining us! <br/>" +
                       $"TFBS team <br/>" +
                       $"</h4>",
                IsBodyHtml = true,
            };

            smtpServer.Send(mail);
        }
    }
}