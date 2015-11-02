
namespace PhotoContest.Data
{
    using System.Text.RegularExpressions;

    using PhotoContest.Data.Repositories;
    using PhotoContest.Models;


    public interface IPhotoContestData
    {
        IRepository<User> Users { get; }

        IRepository<Category> Categories { get; }

        IRepository<Contest> Contests { get; }

        IRepository<Comment> Comments { get; }

        IRepository<Image> Images { get; }
        
        IRepository<Prize> Prizes { get; }
        
        IRepository<Rating> Ratings { get; }

        int SaveChanges();
    }
}
