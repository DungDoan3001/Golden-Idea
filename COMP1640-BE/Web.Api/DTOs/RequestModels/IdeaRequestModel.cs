using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Web.Api.Extensions;

namespace Web.Api.DTOs.RequestModels
{
    public class IdeaRequestModel
    {
        [Required(ErrorMessage = "Must provide a title.")]
        public string Title { get; set; }

        public string Content { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Must choose if the idea is anonymous.")]
        public bool IsAnonymous { get; set; } = true;

        [Required(ErrorMessage = "Must provide a user identity")]
        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "Must provide a category identity")]
        public Guid? CategoryId { get; set; }

        [Required(ErrorMessage = "Must provide a topic identity")]
        public Guid? TopicId { get; set; }

        [DataType(DataType.Upload)]
        [AllowedNumberOfFile(5)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".txt", ".png", ".jpg", ".doc", ".pdf" })]
        public List<IFormFile> UploadFiles { get; set; }
    }
}
