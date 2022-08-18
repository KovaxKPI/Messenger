using Microsoft.AspNetCore.SignalR;

namespace Messenger.Models
{
    public class ChatHub : Hub
    {
        /*public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }*/
        public async Task SendMessage(string user, string message, string to)
        {
            if(string.IsNullOrEmpty(to))
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                if (Context.UserIdentifier != to)
                    await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", user, message);
                await Clients.User(to).SendAsync("ReceiveMessage", user, message);
            }
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"Hello {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
    }
}
