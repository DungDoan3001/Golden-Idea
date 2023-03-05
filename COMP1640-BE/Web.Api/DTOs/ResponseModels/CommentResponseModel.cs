﻿using System;

namespace Web.Api.DTOs.ResponseModels
{
    public class CommentResponseModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}