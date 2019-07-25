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

namespace FindMeAJob.Controllers
{
    public class JobDTO
    {
        public String URL { get; set; }
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
        public async Task<ActionResult<IEnumerable<Jobs>>> GetJobs()
        {
            return await _context.Jobs.ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
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
        public async Task<ActionResult<Jobs>> PostJob(string jobSearch, string location)
        {
            Jobs job = new Jobs();
            try
            {
                int length = JobHelper.jobLength(jobSearch, location);
                for (int i = 0; i < length; i++)
                {
                    job = JobHelper.GetJobInfo(jobSearch, location)[i];
                    _context.Jobs.Add(job);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                return BadRequest("Invalid YouTube URL");
            }
            int k = job.JobId;
            return CreatedAtAction("GetJobs", new { id = job.JobId }, job);
        }


        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
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
        public JobDTO Patch(int id, [FromBody]JsonPatchDocument<JobDTO> jobPatch)
        {
            //get original video object from the database
            Jobs originJob = jobRepository.GetJobsByID(id);
            //use automapper to map that to DTO object
            JobDTO jobDTO = _mapper.Map<JobDTO>(originJob);
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
