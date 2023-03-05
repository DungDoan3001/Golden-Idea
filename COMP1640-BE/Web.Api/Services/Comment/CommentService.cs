﻿using System.Threading.Tasks;
using System;
using Web.Api.Data.Repository;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;
using Web.Api.Data.Context;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Web.Api.DTOs;
using Web.Api.DTOs.ResponseModels;
using Web.Api.DTOs.RequestModels;
using Microsoft.AspNetCore.Identity;

namespace Web.Api.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Entities.Comment> _commentRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Entities.User> _userManager;
        public CommentService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper, UserManager<Entities.User> userManager)
        {
            _unitOfWork = unitOfWork;
            _commentRepo = unitOfWork.GetBaseRepo<Entities.Comment>();
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CommentResponseModel> Create(CommentRequestModel comment)
        {
            try
            {
                var idea = await _context.Ideas.FindAsync(comment.IdeaId);
                if (idea == null)
                {
                    throw new Exception("Can not find idea!");
                }
                var user = await _userManager.FindByNameAsync(comment.Username);
                if (user == null)
                {
                    throw new Exception("Can not find the user!");
                }
                var addComment = _commentRepo.Add(new Entities.Comment
                {
                    UserId = user.Id,
                    IdeaId = comment.IdeaId,
                    Content = comment.Content,
                    IsAnonymous = comment.IsAnonymous
                });
                await _unitOfWork.CompleteAsync();
                var result = _mapper.Map<CommentResponseModel>(addComment);
                result.Username = user.UserName;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<CommentResponseModel>> GetAllCommentOfIdea(Guid ideaId)
        {
            try
            {
                var comments = await _context.Comments
                    .Where(x => x.IdeaId == ideaId)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync();
                var result = _mapper.Map<List<CommentResponseModel>>(comments);
                //Get username
                int i = 0;
                foreach(var item in comments)
                {
                    var user = await _userManager.FindByIdAsync(item.UserId.ToString());
                    if(user == null)
                    {
                        throw new Exception("Can not find the user!");
                    }
                    if(i < result.Count)
                    {
                        result[i].Username = user.UserName;
                        i++;
                    }
                }
                
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}