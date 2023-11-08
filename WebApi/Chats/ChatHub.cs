using Microsoft.AspNetCore.SignalR;

namespace ChatAppApi.Chats
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Receive", message);
        }
    }
}
 