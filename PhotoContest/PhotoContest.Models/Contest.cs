
namespace PhotoContest.Models
{
    using PhotoContest.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Contest
    {
        private ICollection<Image> images;
        private ICollection<Prize> prizes;
        //private ICollection<User> participants;

        public Contest()
        {
            this.images = new HashSet<Image>();
            this.prizes = new HashSet<Prize>();
            //this.participants = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Flag Flag { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? NumberOfPrizes { get; set; }

        public int? ParticipantsLimit { get; set; }

        [Required]
        public RewardStrategy RewardStrategy { get; set; }

        [Required]
        public VotingStrategy VotingStrategy { get; set; }

        [Required]
        public ParticipationStrategy ParticipationStrategy { get; set; }

        [Required]
        public DeadlineStrategy DeadlineStrategy { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<Prize> Prizes
        {
            get { return this.prizes; }
            set { this.prizes = value; }
        }

        //public virtual ICollection<User> Participants
        //{
        //    get { return this.participants; }
        //    set { this.participants = value; }
        //}
    }
}
