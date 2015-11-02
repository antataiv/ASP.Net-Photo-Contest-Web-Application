namespace PhotoContest.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Prize
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PrizeName { get; set; }

        [Required]
        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        [Required]
        public int Position { get; set; }

        //public string WinnerId { get; set; }

        public virtual User Winner { get; set; }
    }
}
