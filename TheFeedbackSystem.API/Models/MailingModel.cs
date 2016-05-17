using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheFeedbackSystem.API.Models
{
    public class MailingModel
    {
        public IdentityUser User { get; set; }
        public string ConfirmationKey { get; set; }
    }
}