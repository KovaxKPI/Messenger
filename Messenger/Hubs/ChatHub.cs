using Messenger.Database;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Hubs
{
    public class ChatHub : Hub
    {
        /*public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }*/
        private readonly ApplicationContext db;
        public ChatHub(ApplicationContext context)
        {
            db = context;
        }
        /*public async Task SendMessageGroup(string user, string message, string roomName)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendPrivateMessage(string user, string message, string to)
        {
            if (Context.UserIdentifier != to)
                await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", user, message);
            await Clients.User(to).SendAsync("ReceiveMessage", user, message);
        }*/
        public async Task SendMessage(string user, string message, string to, string roomName)
        {
            if (!string.IsNullOrEmpty(to))
            {
                if (Context.UserIdentifier != to)
                    await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", user, message);
                await Clients.User(to).SendAsync("ReceiveMessage", user, message);
            }
            else if (!string.IsNullOrEmpty(roomName))
            {
                await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
            }
            else if (!string.IsNullOrEmpty(roomName) && !string.IsNullOrEmpty(to))
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            /*if (!string.IsNullOrEmpty(roomName) && string.IsNullOrEmpty(to))
            {
                await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
            }
            else if(!string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(roomName))
            {
                if (Context.UserIdentifier != to)
                    await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", user, message);
                await Clients.User(to).SendAsync("ReceiveMessage", user, message);
            }*/

            //await Clients.All.SendAsync("ReceiveMessage", user, message);

        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"Hello {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
        public async Task ConnectRoom(string username, string roomName)
        {
            db.Groups.FirstOrDefault(x => x.Name == roomName).Users.Add(db.Users.FirstOrDefault(y => y.Nickname == username));
            await db.SaveChangesAsync();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.All.SendAsync("Notify", $"{username} connected to {roomName} room");
        }
        public async Task DisconnectRoom(string username, string roomName)
        {
            db.Groups.FirstOrDefault(x => x.Name == roomName).Users.Remove(db.Users.FirstOrDefault(y => y.Nickname == username));
            await db.SaveChangesAsync();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.All.SendAsync("Notify", $"{username} disconnected from {roomName} room");
        }
    }
}
