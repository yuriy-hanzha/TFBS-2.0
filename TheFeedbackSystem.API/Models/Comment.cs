using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace TheFeedbackSystem.API.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Feeds")]
    public class Comment
    {
        [Key]
        public string Id { get; set; }
        public string Text { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public string LikedBy { get; set; }
        public string DislikedBy { get; set; }
        public Client Author { get; set; }
        public string AuthorId { get; set; }
    }
}