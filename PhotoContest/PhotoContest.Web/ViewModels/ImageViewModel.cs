namespace PhotoContest.Web.ViewModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class ImageViewModel : IMapFrom<Image>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Author")]
        public string User { get; set; }

        public string PictureUrl { get; set; }

        public int Rating { get; set; }

        public string VotingStrategy { get; set; }

        public string ParticipationStrategy { get; set; }

        public ICollection<string> VotedUsers { get; set; }

        public ICollection<string> UsersInContest { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(i => i.User, conf => conf.MapFrom(m => m.Author.UserName));
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(i => i.Rating, conf => conf.MapFrom(m => m.Ratings.Count));
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(i => i.VotingStrategy, conf => conf.MapFrom(m => m.Contest.VotingStrategy));
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(i => i.ParticipationStrategy, conf => conf.MapFrom(m => m.Contest.ParticipationStrategy));
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(i => i.VotedUsers, conf => conf.MapFrom(m => m.Ratings.Select(r => r.Author.Id)));
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(i => i.UsersInContest, conf => conf.MapFrom(m => m.Contest.Participants.Select(u => u.Id)));

        }
    }
}