using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoContest.Common.Mappings;
using PhotoContest.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace PhotoContest.Web.ViewModels
{
    public class MyImagesViewModel : IMapFrom<Image>
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }
    }
}