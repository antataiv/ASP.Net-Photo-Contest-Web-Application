using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoContest.Common.Mappings;
namespace PhotoContest.Web.ViewModels
{
    public class WinnerImagesViewModel : IMapFrom<Image> ,IHaveCustomMappings
    {
        public int Id { get; set; }

        public string User { get; set; }

        public double Rating { get; set; }

        public string PictureUrl { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Image, WinnerImagesViewModel>()
                .ForMember(x => x.Rating, cnf => cnf.MapFrom(i =>i.Ratings.Any() ? i.Ratings.Sum(r => r.Value) :0));

            configuration.CreateMap<Image, WinnerImagesViewModel>()
                .ForMember(x => x.User, cnf => cnf.MapFrom(i => i.Author.UserName));
        }
    }
}