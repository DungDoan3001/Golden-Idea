﻿using System;

namespace Web.Api.DTOs.RequestModels
{
    public class CommentRequestModel
    {
        public Guid IdeaId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
