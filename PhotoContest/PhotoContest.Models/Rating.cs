namespace PhotoContest.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Value { get; set; }

        //[Required]
        //public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        [Required]
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}
