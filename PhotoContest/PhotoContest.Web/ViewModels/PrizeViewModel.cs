namespace PhotoContest.Web.ViewModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;

    public class PrizeViewModel : IMapFrom<Prize>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string PrizeName { get; set; }

        public int ContestId { get; set; }

        public int Position { get; set; }

        public string WinnerId { get; set; }

        public string Username { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Prize, PrizeViewModel>()
               .ForMember(x => x.WinnerId, cnf => cnf.MapFrom(u => u.Id));

            configuration.CreateMap<Prize, PrizeViewModel>()
               .ForMember(x => x.Username, cnf => cnf.MapFrom(u => u.Winner.UserName));
        }
     }
}