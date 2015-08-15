using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASH.DTO;

namespace ASH.DataAccess
{
    public static class EntityDomainMapper
    {
        public static Job ToEntity(this DTOJob dtoJob)
        {
            Job entityJob = new Job()
            {
                JobNumber = dtoJob.JobNumber,
                status = dtoJob.status,
                Path = dtoJob.Path
            };
            return entityJob;
        }
        public static JobTracking ToEntity(this DTOJobTracking dtoJobTracking)
        {
            JobTracking entityJobTracking = new JobTracking()
            {
                JobNumber = dtoJobTracking.JobNumber,
                status = dtoJobTracking.status,
                Path = dtoJobTracking.Path
            };
            return entityJobTracking;
        }
    }
}
