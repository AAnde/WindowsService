using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASH.DTO
{
    public class DTOJob
    {
        public long JobNumber { get; set; }
        public Nullable<int> status { get; set; }
        public string Path { get; set; }
    }
}
