using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoContest.Common.Mappings;
using PhotoContest.Models;

namespace PhotoContest.Web.ViewModels
{
    public class CommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string AuthorUserName { get; set; }
    }
}