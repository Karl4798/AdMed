using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models
{
    public class Post
    {

        public int Id { get; set; }
        [DisplayName("Post Title")]
        [Required] public string PostName { get; set; }
        [DisplayName("Post Description")]
        [Required] public string PostDescription { get; set; }
        [DisplayName("Post Content")]
        [Required] public string Content { get; set; }
        public DateTime TimeStamp { get; set; }

        public Post()
        {
            TimeStamp = DateTime.Now;
        }

    }
}
