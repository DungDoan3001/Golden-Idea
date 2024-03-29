﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using Web.Api.DTOs;
using System.Linq;
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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<DepartmentRequestModel, Department>();

            // Topic
            CreateMap<Entities.Topic, TopicResponseModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.TotalIdea, opt => opt.MapFrom(src => src.Ideas.Count))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));
            CreateMap<TopicRequestModel, Entities.Topic>();

            // Category
            CreateMap<Entities.Category, CategoryResponseModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<CategoryRequestModel, Entities.Category>();

            // Role
            CreateMap<IdentityRole<Guid>, RoleResponseModel>();

            //User
            CreateMap<UserRequestModel, User>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.File));
            CreateMap<User, UserRequestModel>()
                .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.Avatar));
            CreateMap<User, UserResponseModel>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar));

            // Idea
            CreateMap<Idea, IdeaResponseModel>()
                .AfterMap((src, dest) =>
                {
                    // Map view
                    dest.View = src.Views.Count();
                    // Map Upvote
                    dest.UpVote = src.Reactions.Where(x => x.React == 1).Count();
                    // Map Downvote
                    dest.DownVote = src.Reactions.Where(x => x.React == -1).Count();
                });
            CreateMap<Idea, IdeaForZipResponseModel>()
                .AfterMap((src, dest) =>
                {
                    // Map view
                    dest.View = src.Views.Count();
                    // Map Upvote
                    dest.UpVote = src.Reactions.Where(x => x.React == 1).Count();
                    // Map Downvote
                    dest.DownVote = src.Reactions.Where(x => x.React == -1).Count();
                })
                .ForMember(dest => dest.Category_name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.User_Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Topic.Name));
            //
            CreateMap<Topic, IdeaResponseModel_Topic>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));
            CreateMap<User, IdeaResponseModel_User>();
            CreateMap<Category, IdeaResponseModel_Category>();
            CreateMap<File, IdeaResponseModel_File>()
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .AfterMap((src, dest) =>
                {
                    if(src.Format== null)
                    {
                        dest.FileExtension = src.PublicId.Split(".").Last();
                    } else { dest.FileExtension = src.Format; }
                });
            CreateMap<IdeaRequestModel, Idea>();
            // Reaction
            CreateMap<Reaction, ReactionResponseModel>();
            CreateMap<Reaction, GetUserReactionResponseModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));

            //Comment
            CreateMap<Comment, CommentResponseModel>();

            //View
            CreateMap<View, ViewResponseModel>();

            // Chart
            CreateMap<Idea, IdeaForChartResponseModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .AfterMap((src, dest) =>
                {
                    dest.TotalView = src.Views.Count;
                    dest.TotalComment = src.Comments.Count;
                    dest.TotalUpVote = src.Reactions.Where(x => x.React == 1).Count();
                    dest.TotalDownVote = src.Reactions.Where(x => x.React == -1).Count();
                });
        }
    }
}
