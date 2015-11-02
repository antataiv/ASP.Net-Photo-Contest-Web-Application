using System.ComponentModel.DataAnnotations;
using PhotoContest.Web.Validators;

namespace PhotoContest.Web.Models.BindingModels
{
    using System.Web;

    public class ImageBindingModel
    {
        [Required (ErrorMessage = "Please choose a title for your image.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [ValidateImageSize(ErrorMessage = "Your file exceeds the maximum size limit of 4MB")]
        [ValidateImageExtension(ErrorMessage = "Your file does not meet the file format requirements. Allowed formats are \".jpg\", \".jpeg\", \".png\", \".bmp\" and \".gif\".")]
        public HttpPostedFileBase Upload { get; set; } 
    }
}