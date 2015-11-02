namespace PhotoContest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }

        //[Required]
        //public string AuthorId { get; set; }

        public virtual User Author { get; set; }
    }
}
