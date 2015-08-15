using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASH.DTO;

namespace ASH.DataAccess
{
    public class DALOperations
    {
        public void AddJob(DTOJob job)
        {
            using (SampleDBEntities Entities = new SampleDBEntities())
            {
                var jobEntity = job.ToEntity();
                Entities.Jobs.Add(jobEntity);
                Entities.SaveChanges();
            }
        }
        public void UpdateJob(DTOJob job)
        {
           using(SampleDBEntities Entities = new SampleDBEntities())
           {
               var result = Entities.Jobs.FirstOrDefault(x => x.JobNumber == job.JobNumber);
               if (result != null)
               {
                   result.status = job.status;
               }
               Entities.SaveChanges();
           }
        }
        public void SaveToJobTracking(DTOJobTracking jobTracking)
        {
            using (var Entities = new SampleDBEntities())
            {
                var jobTrackingEntity = jobTracking.ToEntity();
                Entities.JobTrackings.Add(jobTrackingEntity);
                Entities.SaveChanges();
            }
        }
        public int GetJobNumber()
        {
            using (var Entites = new SampleDBEntities())
            {
                 int jobnumber = Convert.ToInt32(Entites.spGetUniqueId().FirstOrDefault());
                 return jobnumber;
            }
            
        }
    }
}
