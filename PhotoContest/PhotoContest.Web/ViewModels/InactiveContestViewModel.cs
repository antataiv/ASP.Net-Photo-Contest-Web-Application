namespace PhotoContest.Web.ViewModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;

    public class InactiveContestViewModel : IMapTo<Contest>, IHaveCustomMappings
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Contest, InactiveContestViewModel>()
                .ForMember(x => x.Flag, cnf => cnf.MapFrom(m => m.Flag.ToString()));
        }
    }
}