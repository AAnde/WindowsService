using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASH.DataAccess;
using ASH.DTO;

namespace ASH.Business
{
    public class BLOperations
    {
        DALOperations objDal = null;
        public BLOperations()
        {
            objDal = new DALOperations();
        }
        public void AddJob(DTOJob job)
        {
            objDal.AddJob(job);
        }
        public void UpdateJob(DTOJob job)
        {
            objDal.UpdateJob(job);
        }
        public void AddJobTracking(DTOJobTracking jobtrack)
        {
            objDal.SaveToJobTracking(jobtrack);
        }
        public int GetJobNumber()
        {
            return objDal.GetJobNumber();
        }
    }
}
