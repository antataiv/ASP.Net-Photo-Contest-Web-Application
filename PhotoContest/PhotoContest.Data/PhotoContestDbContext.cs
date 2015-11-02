using Microsoft.AspNet.Identity.EntityFramework;
using PhotoContest.Models;
using System.Data.Entity;
using PhotoContest.Data.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PhotoContest.Data
{
    public class PhotoContestDbContext : IdentityDbContext<User>, IPhotoContestDbContext
    {
        public PhotoContestDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoContestDbContext, Configuration>());
        }

        public static PhotoContestDbContext Create()
        {
            return new PhotoContestDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contest>()
                .HasMany(c => c.Participants)
                .WithMany(u => u.PatricipatingContests)
                .Map(m =>
                {
                    m.ToTable("ContestsUsers");
                    m.MapLeftKey("ContestId");
                    m.MapRightKey("ParticipantId");
                });

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Contest> Contests { get; set; }

        public IDbSet<Image> Images { get; set; }

        public IDbSet<Prize> Prizes { get; set; }

        public IDbSet<Rating> Ratings { get; set; }
    }
}
