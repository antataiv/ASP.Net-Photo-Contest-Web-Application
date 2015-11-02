
namespace PhotoContest.Data
{
    using System.Data.Entity;

    using PhotoContest.Models;

    public interface IPhotoContestDbContext
    {
        IDbSet<Contest> Contests { get; set; }

        IDbSet<Category> Categories { get; set; }

        IDbSet<Comment> Comments { get; set; }

        IDbSet<Image> Images { get; set; }
        
        IDbSet<Prize> Prizes { get; set; }
        
        IDbSet<Rating> Ratings { get; set; }

        int SaveChanges();
    }
}
