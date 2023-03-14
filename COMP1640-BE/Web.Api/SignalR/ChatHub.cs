using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Web.Api.DTOs.RequestModels;
using Web.Api.Services.Comment;

namespace Web.Api.SignalR
{
    public class ChatHub : Hub
    {
        private readonly ICommentService _commentService;
        public ChatHub(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task SendComment(CommentRequestModel userComment)
        {
            var comment = await _commentService.Create(userComment);
            await Clients.Group(userComment.IdeaId.ToString())
                .SendAsync("ReceiveComment", comment);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var ideaId = httpContext.Request.Query["ideaId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, ideaId);
            var comments = await _commentService.GetAllCommentOfIdea(Guid.Parse(ideaId));
            await Clients.Caller.SendAsync("LoadComments", comments);
        }
    }
}
