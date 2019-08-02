using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindMeAJob.Model;
using FindMeAJob.Helper;
using FindMeAJob.DAL;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using Microsoft.AspNetCore.Cors;

namespace FindMeAJob.Controllers
{
    public class JobDTO
    {
        public String jobSearch { get; set; }
        public String location { get; set; }
        public String from { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private IJobRepository jobRepository;
        private readonly IMapper _mapper;
        private readonly jobsContext _context;

        public JobsController(jobsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            this.jobRepository = new JobRepository(new jobsContext());
        }

        // GET: api/Jobs
        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<IEnumerable<Jobs>>> GetJobs()
        {
            return await _context.Jobs.ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<Jobs>> GetJobs(int id)
        {
            var jobs = await _context.Jobs.FindAsync(id);

            if (jobs == null)
            {
                return NotFound();
            }

            return jobs;
        }

        // PUT: api/Jobs/5
        [HttpPut("{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<IActionResult> PutJobs(int id, Jobs jobs)
        {
            if (id != jobs.JobId)
            {
                return BadRequest();
            }

            _context.Entry(jobs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        [HttpPost]
        [EnableCors("AllowAllHeaders")]
        // public async Task<ActionResult<Jobs>> PostJob([FromBody]JobDTO data)                 (1)
        public async Task<ActionResult<IEnumerable<Jobs>>> PostJob([FromBody]JobDTO data)
        {

            String jobSearch = data.jobSearch;
            String location = data.location;
            String from = data.from;
            List<Jobs> newJobs = new List<Jobs>();

            if (from == "seek")
            {
                Jobs job = new Jobs();
                try
                {
                    //int length = JobHelper.jobLength(jobSearch, location);
                    for (int i = 0; i < 10; i++)
                    {
                         //job.Add(JobHelper.GetJobInfo(jobSearch, location, from)[i]);
                         job = JobHelper.GetJobInfo(jobSearch, location, from)[i];
                        newJobs.Add(job);
                        _context.Jobs.Add(job);
                        await _context.SaveChangesAsync();
                    }
                }
                catch
                {
                    return BadRequest("Invalid URL");
                }
            }
            //else if(from == "indeed")
            //{
            //    try
            //    {
            //        for (int i = 0; i < 10; i++)
            //        {
            //            job.Add(JobHelper.GetJobInfo(jobSearch, location, from)[i]);
            //        }
            //    }
            //    catch
            //    {
            //        return BadRequest("Invalid URL");
            //    }
            //}
            else
            {
                return BadRequest("No Valid URL");

            }

             return newJobs;
             //return await _context.Jobs.ToListAsync();

           // return CreatedAtAction("GetJobs", new { id = job.JobId }, job);               (1)
        }


        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<Jobs>> DeleteJobs(int id)
        {
            var jobs = await _context.Jobs.FindAsync(id);
            if (jobs == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(jobs);
            await _context.SaveChangesAsync();

            return jobs;
        }

        //PUT with PATCH to handle isFavourite
        [HttpPatch("update/{id}")]
        [EnableCors("AllowAllHeaders")]
        public JobsDTO Patch(int id, [FromBody]JsonPatchDocument<JobsDTO> jobPatch)
        {
            //get original video object from the database
            Jobs originJob = jobRepository.GetJobsByID(id);
            //use automapper to map that to DTO object
            JobsDTO jobDTO = _mapper.Map<JobsDTO>(originJob);
            //apply the patch to that DTO
            jobPatch.ApplyTo(jobDTO);
            //use automapper to map the DTO back ontop of the database object
            _mapper.Map(jobDTO, originJob);
            //update video in the database
            _context.Update(originJob);
            _context.SaveChanges();
            return jobDTO;
        }

        private bool JobsExists(int id)
        {
            return _context.Jobs.Any(e => e.JobId == id);
        }
    }
}
