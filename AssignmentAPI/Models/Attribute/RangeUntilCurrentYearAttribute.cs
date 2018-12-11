using System;
using System.ComponentModel.DataAnnotations;

namespace AssignmentAPI.Models.Attribute
{
    public class RangeUntilCurrentYearAttribute: RangeAttribute
    {
        public RangeUntilCurrentYearAttribute(int minimum): base(minimum, DateTime.Now.Year)
        {
        }
    }
}