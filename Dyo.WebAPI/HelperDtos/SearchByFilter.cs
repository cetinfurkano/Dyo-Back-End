using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dyo.WebAPI.HelperDtos
{
    public class SearchByFilter
    {
        public string CategoryName { get; set; }
        public List<int> EducationTypes { get; set; }
        public List<int> Branches { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
    }
}
