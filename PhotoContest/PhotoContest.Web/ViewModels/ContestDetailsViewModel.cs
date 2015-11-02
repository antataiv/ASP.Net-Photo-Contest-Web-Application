namespace PhotoContest.Web.ViewModels
{
    using AutoMapper;
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ContestDetailsViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string CreatorName { get; set; }

        public int ParticipatingImages { get; set; }

        public Flag Flag { get; set; }

        public bool UserPatricipates { get; set; }

        public int Participants { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(m => m.ParticipatingImages, conf => conf.MapFrom(i => i.Images.Count));
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(m => m.Participants, conf => conf.MapFrom(p => p.Participants.Count));
        }
    }
}