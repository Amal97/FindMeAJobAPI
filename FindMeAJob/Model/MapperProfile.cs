using AutoMapper;
using FindMeAJob.Controllers;

namespace FindMeAJob.Model
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<Jobs, JobsDTO>();
            CreateMap<JobsDTO, Jobs>();
        }
    }
}
