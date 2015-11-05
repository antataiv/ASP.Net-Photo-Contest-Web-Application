namespace PhotoContest.Web.Models.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;
    using PhotoContest.Common.Mappings;
    public class EditContestBindingModel : IMapTo<Contest>
    {
  
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "The {0} should be between 0 and {1} characters long.")]
        public string Name { get; set; }

        [Display(Name = "Enter Deadline Date: ")]
        public DateTime? EndDate { get; set; }
        public int? ParticipantsLimit { get; set; }
        public DeadlineStrategy DeadlineStrategy { get; set; }       
    }
}