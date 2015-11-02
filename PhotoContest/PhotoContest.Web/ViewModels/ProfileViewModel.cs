using System.Collections.Generic;

namespace PhotoContest.Web.ViewModels
{
    public class ProfileViewModel
    {
        public ICollection<MyContestsViewModel> MyContests { get; set; }

        public UserProfileViewModel UserProfileViewModel { get; set; }
    }
}