namespace PhotoContest.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class User : IdentityUser
    {
        private ICollection<Image> images;
        private ICollection<Rating> ratings;
        private ICollection<Comment> comments;
        private ICollection<Prize> prizes;
        private ICollection<Contest> participatingContests;

        public User()
        {
            this.images = new HashSet<Image>();
            this.ratings = new HashSet<Rating>();
            this.comments = new HashSet<Comment>();
            this.prizes = new HashSet<Prize>();
            this.participatingContests = new HashSet<Contest>();
        }

        public string ProfileImageUrl { get; set; }

        //public string ProfileImagePath { get; set; }

        //public string ThumbnailPath { get; set; }

        //public string ThumbnailUrl { get; set; }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<Rating> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Prize> Prizes
        {
            get { return this.prizes; }
            set { this.prizes = value; }
        }

        //public int PatricipatingContestId { get; set; }

        public virtual ICollection<Contest> PatricipatingContests
        {
            get { return this.participatingContests; }
            set { this.participatingContests = value; }
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
