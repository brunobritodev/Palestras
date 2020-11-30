using AutoMapper;
using DevWeek.Models;
using DevWeek.ViewModels;

namespace DevWeek.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Applicant, ApplicantViewModel>();
        }


    }
}
