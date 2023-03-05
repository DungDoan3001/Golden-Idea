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

            // Idea
            CreateMap<Idea, IdeaResponeModel>();
            CreateMap<Topic, IdeaResponeModel_Topic>();
            CreateMap<User, IdeaResponeModel_User>();
            CreateMap<Category, IdeaResponeModel_Category>();
            CreateMap<File, IdeaResponeModel_File>()
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
                .AfterMap((src, dto) =>
                {
                    string file = src.PublicId.Split("GoldenIdeaRaw/")[1];
                    dto.FileName = file.Split('.')[0];
                    dto.FileExtention= file.Split(".")[1];
                });
            CreateMap<IdeaRequestModel, Idea>();

            // Reaction
            CreateMap<Reaction, ReactionResponseModel>();
        }
    }
}
