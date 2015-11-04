using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoContest.Common.Mappings;
using PhotoContest.Models;
using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Web.Models.BindingModels
{
    public class CreatePrizesBindingModel : IMapTo<Prize>
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "The {0} should be between 0 and {1} characters long.")]
        public string PrizeName { get; set; }

        public int ContestId { get; set; }

        [Required]
        public int Position { get; set; }

    }
}