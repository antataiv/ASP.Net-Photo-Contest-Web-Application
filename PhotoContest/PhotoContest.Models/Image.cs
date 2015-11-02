namespace PhotoContest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        private ICollection<Rating> ratings;
        private ICollection<Comment> comments;

        public Image()
        {
            this.ratings = new HashSet<Rating>();
            this.comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string ThumbnailUrl { get; set; }


        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        //[Required]
        //public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public virtual ICollection<Rating> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }
    }
}
