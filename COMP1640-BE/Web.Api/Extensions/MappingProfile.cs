using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using Web.Api.DTOs.RequestModels;
using Web.Api.DTOs.ResponseModels;
using Web.Api.Entities;

namespace Web.Api.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department
            CreateMap<Department, DepartmentResponseModel>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id));
            CreateMap<DepartmentRequestModel, Department>();

            // Topic
            CreateMap<Entities.Topic, TopicResponseModel>()
                .ForMember(dest => dest.TopicId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<TopicRequestModel, Entities.Topic>();

            // Topic
            CreateMap<Entities.Category, CategoryResponseModel>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id));
            CreateMap<CategoryRequestModel, Entities.Category>();

            // Role
            CreateMap<IdentityRole<Guid>, RoleResponseModel>();

            // Authentication
            CreateMap<UserForRegistrationRequestModel, User>();
            CreateMap<User, UserForRegistrationResponseModel>();

            //User
            CreateMap<UserRequestModel, User>();
            CreateMap<User, UserRequestModel>();
            CreateMap<User, UserResponseModel>();

            //Reaction
            CreateMap<Reaction, ReactionResponseModel>();
        }
    }
}
