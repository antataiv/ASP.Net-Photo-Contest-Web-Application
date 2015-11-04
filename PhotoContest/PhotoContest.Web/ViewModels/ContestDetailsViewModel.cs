using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Web.ViewModels
{
    using AutoMapper;
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ContestDetailsViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        //New Version
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name="Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Creator")]
        public string CreatorName { get; set; }

        [Display(Name = "# Images")]
        public int ParticipatingImages { get; set; }

        public Flag Flag { get; set; }

        public ICollection<string> ParticipatingUsers { get; set; }

        public int Participants { get; set; }

        public string ParticipationStrategy { get; set; }

        public int? NumberOfPrizes { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(m => m.Images, conf => conf.MapFrom(i => i.Images.Where(f => f.isDeleated == false)));
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(m => m.ParticipatingImages, conf => conf.MapFrom(i => i.Images.Where(f=>f.isDeleated==false).Count()));
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(m => m.Participants, conf => conf.MapFrom(p => p.Participants.Count));
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(m => m.ParticipationStrategy, conf => conf.MapFrom(c => c.ParticipationStrategy));
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                .ForMember(m => m.ParticipatingUsers, conf => conf.MapFrom(p => p.Participants.Select(u => u.Id)));
        }
    }
}