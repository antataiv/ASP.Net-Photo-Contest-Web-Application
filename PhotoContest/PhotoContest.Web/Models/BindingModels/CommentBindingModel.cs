using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PhotoContest.Common.Mappings;
using System.Web;

namespace PhotoContest.Web.Models.BindingModels
{
    public class CommentBindingModel : IMapTo<Comment>
    {
        public string UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public int ImageId { get; set; }

        
    }
}