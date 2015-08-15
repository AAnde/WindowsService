using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASH.DTO
{
    public class DTOJobTracking
    {   
        public Nullable<long> JobNumber { get; set; }
        public Nullable<int> status { get; set; }
        public string Path { get; set; }
    }
}
