using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace AssignmentAPI.Models
{
    public class MatchModel
    {
        //[BindRequired]
        [Required(AllowEmptyStrings = false)]
        [MinLength(8)]
        public string MatchDateTime { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        //[BindRequired]
        public string MatchTitle { get; set; }
    }
}