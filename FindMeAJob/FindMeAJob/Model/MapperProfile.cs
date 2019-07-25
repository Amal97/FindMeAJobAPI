using AutoMapper;
using FindMeAJob.Controllers;

namespace FindMeAJob.Model
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<Jobs, JobDTO>();
            CreateMap<JobDTO, Jobs>();
        }
    }
}
