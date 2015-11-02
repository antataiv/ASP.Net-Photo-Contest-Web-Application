using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PhotoContest.Common.Mappings;
using PhotoContest.Models;

namespace PhotoContest.Web.ViewModels 
{
    public class UserProfileViewModel : IMapTo<User>,IHaveCustomMappings
    {
        public string ProfileImageUrl { get; set; }

        public string ProfileImagePath { get; set; }

        public string ThumbnailPath { get; set; }

        public string ThumbnailUrl { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserProfileViewModel>()
                .ForMember(x => x.UserName, cnf => cnf.MapFrom(m => this.UserName));

            configuration.CreateMap<User, UserProfileViewModel>()
               .ForMember(x => x.Email, cnf => cnf.MapFrom(m => m.Email));

            configuration.CreateMap<User, UserProfileViewModel>()
              .ForMember(x => x.PhoneNumber, cnf => cnf.MapFrom(m => m.PhoneNumber));
        }
    }
}