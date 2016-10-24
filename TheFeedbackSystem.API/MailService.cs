using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Microsoft.AspNet.Identity;
using PreMailer.Net;
using RazorEngine.Templating;
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
            smtpServer = new SmtpClient("server.address")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("server.email@mail.net", "password"),
                EnableSsl = false
            };
        }

        public void SendWelcomeMessage(IdentityUser user)
        {
            //var templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "WelcomeEmail.cshtml");
            //var templateService = new TemplateService();
            //var emailHtmlBody = templateService.Parse(
            //    File.ReadAllText(templateFilePath), new MailingModel() { User = user }, null, "WelcomeEmail");
            //var inlinedHtmlBody = PreMailer.Net.PreMailer.MoveCssInline(emailHtmlBody).Html;

            //MailMessage mail = new MailMessage()
            //{
            //    From = new MailAddress("thefeedbacksystem@radacode.net"),
            //    To = { user.Email },
            //    Subject = "Welcome | TFBS",
            //    Body = inlinedHtmlBody,
            //    IsBodyHtml = true,
            //};

            //smtpServer.Send(mail);
        }

        public void SendConfirmationMessage(IdentityUser user)
        {
            //string userId = _userManager.FindByName(user.UserName).Id;
            //string key = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            //_ctx.ConfirmationKeys.Add(new ConfirmationKey()
            //{
            //    UserId = userId,
            //    Key = key
            //});
            //_ctx.SaveChanges();

            //var templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "ConfirmationEmail.cshtml");
            //var templateService = new TemplateService();
            //var emailHtmlBody = templateService.Parse(
            //    File.ReadAllText(templateFilePath), new MailingModel() { User = user, ConfirmationKey = key }, null, "ConfirmationEmail");
            //var inlinedHtmlBody = PreMailer.Net.PreMailer.MoveCssInline(emailHtmlBody).Html;

            //MailMessage mail = new MailMessage()
            //{
            //    From = new MailAddress("thefeedbacksystem@radacode.net"),
            //    To = { user.Email },
            //    Subject = "The last stage of registration | TFBS",
            //    Body = inlinedHtmlBody,
            //    IsBodyHtml = true,
            //};

            //smtpServer.Send(mail);
        }
    }
}