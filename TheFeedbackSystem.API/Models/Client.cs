using System;
using System.ComponentModel.DataAnnotations;

namespace TheFeedbackSystem.API
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Clients")]
    public class Client
    {
        [Key]
        public string UserId { get; set; }
        public int Likes { get; set; }
        public DateTime LastVisit { get; set; }
        public string Avatar { get; set; }
    }
}