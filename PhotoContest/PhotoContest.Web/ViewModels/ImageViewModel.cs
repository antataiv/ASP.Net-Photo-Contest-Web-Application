namespace PhotoContest.Web.ViewModels
{
    using PhotoContest.Common.Mappings;
    using PhotoContest.Models;

    public class ImageViewModel : IMapFrom<Image>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string PictureUrl { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(i => i.User, conf => conf.MapFrom(m => m.Author.UserName));
        }
    }
}