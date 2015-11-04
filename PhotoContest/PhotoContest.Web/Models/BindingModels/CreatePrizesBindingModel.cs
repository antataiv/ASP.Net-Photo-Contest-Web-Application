namespace PhotoContest.Web.Models.BindingModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;
    using System.ComponentModel.DataAnnotations;
    public class CreatePrizesBindingModel : IMapTo<Prize>
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "The {0} should be between 0 and {1} characters long.")]
        public string PrizeName { get; set; }

        public int ContestId { get; set; }

        [Required(ErrorMessage = "Position can not be empty.")]
        public int Position { get; set; }

    }
}