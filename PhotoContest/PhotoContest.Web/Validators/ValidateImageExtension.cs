namespace PhotoContest.Web.Validators
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class ValidateImageExtension : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file != null)
            {
                var fileExtension = System.IO.Path.GetExtension(file.FileName);

                var imgExtList = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

                if (!imgExtList.Contains(fileExtension.ToLower()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}