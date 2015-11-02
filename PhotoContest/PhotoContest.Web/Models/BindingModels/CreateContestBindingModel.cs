namespace PhotoContest.Web.Models.BindingModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateContestBindingModel : IMapTo<Contest>
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "The {0} should be between 0 and {1} characters long.")]
        public string Name { get; set; }

        [Display(Name = "Enter Deadline Date: ")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Enter Number of Prizes: ")]
        public int NumberOfPrizes { get; set; }

        [Display(Name = "Enter Number of Allowed Participants: ")]
        public int? ParticipantsLimit { get; set; }

        [Display(Name = "Number of Winners")]
        public RewardStrategy RewardStrategy { get; set; }

        public VotingStrategy VotingStrategy { get; set; }

        [Display(Name = "Participation")]
        public ParticipationStrategy ParticipationStrategy { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}