using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoContest.Common.Mappings;
using PhotoContest.Models;

namespace PhotoContest.Web.ViewModels
{
    public class ContestViewModelIndex : IMapTo<Contest>,IHaveCustomMappings
    {
        
        public int Id { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModelIndex>()
                .ForMember(x => x.Flag, cnf => cnf.MapFrom(m => m.Flag.ToString()));
        }
    }
}