namespace PhotoContest.Web.ViewModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;

    public class MyContestsViewModel : IMapFrom<Contest>
    {
        public int Id { get; set; }

        public string Name { get; set; }


    }
}