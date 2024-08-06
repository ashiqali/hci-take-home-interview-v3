using AutoMapper;
using PatientAdministrationSystem.Application.DTOs;
using PatientAdministrationSystem.Application.Entities;

namespace PatientAdministrationSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PatientEntity, PatientDto>().ReverseMap();
            CreateMap<HospitalEntity, HospitalDto>().ReverseMap();
            CreateMap<VisitEntity, VisitDto>().ReverseMap();
        }
    }
}
