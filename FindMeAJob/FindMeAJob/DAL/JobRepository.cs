using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindMeAJob.Model;
using Microsoft.EntityFrameworkCore;

namespace FindMeAJob.DAL
{
    public class JobRepository : IJobRepository, IDisposable
    {
        private jobsContext context;

        public JobRepository(jobsContext context)
        {
            this.context = context;
        }

        public IEnumerable<Jobs> GetJobs()
        {
            return context.Jobs.ToList();
        }

        public Jobs GetJobsByID(int id)
        {
            return context.Jobs.Find(id);
        }

        public void InsertJob(Jobs video)
        {
            context.Jobs.Add(video);
        }

        public void DeleteJob(int jobId)
        {
            Jobs job = context.Jobs.Find(jobId);
            context.Jobs.Remove(job);
        }

        public void UpdateJob(Jobs job)
        {
            context.Entry(job).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
