namespace PhotoContest.Web.ViewModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;

    public class MyContestsViewModel : IMapFrom<Contest>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Flag Flag { get; set; }

        public string CreatorId { get; set; }
        public ParticipationStrategy ParticipationStrategy { get; set; }
        public int? ParticipantsLimit { get; set; }
    }
}