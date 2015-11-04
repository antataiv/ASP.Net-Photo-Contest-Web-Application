namespace PhotoContest.Web.Models.BindingModels
{
    using PhotoContest.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using PhotoContest.Common.Mappings;

    public class CommentBindingModel : IMapTo<Comment>
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "You can not post empty comment.")]
        [MaxLength(255, ErrorMessage = "Your post can be maximum {1} symbols long.")]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public int ImageId { get; set; }


    }
}