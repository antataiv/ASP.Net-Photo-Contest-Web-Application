using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoContest.Common.Mappings;
using PhotoContest.Models;

namespace PhotoContest.Web.ViewModels
{
    public class ImageDetailsViewModel : IMapFrom<Image>,IHaveCustomMappings
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PostedOn { get; set; }

        public string ContestName { get; set; }

        public string AuthorUserName { get; set; }

        public double Rating {get;set;}

        public virtual IEnumerable<CommentViewModel> Comments { get; set; }


        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Image, ImageDetailsViewModel>()
                .ForMember(x => x.Rating, cnf => cnf.MapFrom(m => m.Ratings.Any() ? m.Ratings.Sum(r=>r.Value) : 0));
        }
    }
}