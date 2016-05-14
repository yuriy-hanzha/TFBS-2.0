using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFeedbackSystem.API.Models
{
    public class ConfirmationKey
    {
        [Key]
        public string UserId { get; set; }
        public string Key { get; set; }
    }
}