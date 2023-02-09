using AutoMapper;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponeModels;
using Web.Api.Entities;

namespace Web.Api.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department
            CreateMap<Department, DepartmentResponeModel>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id));
            CreateMap<DepartmentRequestModel, Department>();
        }
    }
}
