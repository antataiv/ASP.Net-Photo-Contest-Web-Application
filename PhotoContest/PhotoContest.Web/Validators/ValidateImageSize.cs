namespace PhotoContest.Web.Validators
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class ValidateImageSize : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file != null)
            {
                if (file.ContentLength > 1 * 3048 * 3048)
                {
                    return false;
                }
            }

            return true;
        }
    }
}