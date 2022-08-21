using Messenger.Database;
using Messenger.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationContext db;
        public ChatHub(ApplicationContext context)
        {
            db = context;
        }
        
        public async Task SendMessage(string user, string message, string to, string roomName)
        {
            Message message1 = new Message() { Content = message, CreatedDate = DateTime.Now, GroupName = roomName, Receiver = to, Sender = user };
            await db.AddAsync(message1);
            await db.SaveChangesAsync();
            if(!string.IsNullOrEmpty(to))
            {
                if (Context.UserIdentifier != to)
                    await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", user, message);
                await Clients.User(to).SendAsync("ReceiveMessage", user, message);
                return;
            }
            if(!string.IsNullOrEmpty(roomName))
            {
                await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
                return;
            }
            await Clients.All.SendAsync("ReceiveMessage", user, message);

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
        public async Task AddToGroups()
        {
            foreach (var group in db.Groups.Include(x => x.Users))
            {
                foreach(var u in group.Users)
                {
                    if(u.Nickname == Context.UserIdentifier)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                    }
                }
                
            }
        }
        public async Task ShowPreviousMessages()
        {
            foreach(var message in db.Messages)
            {
                if (string.IsNullOrEmpty(message.Receiver))
                {
                    if (string.IsNullOrEmpty(message.GroupName))
                    {
                        await Clients.All.SendAsync("ReceiveMessage", message.Sender, message.Content);
                    }
                    else
                    {
                        await Clients.Group(message.GroupName).SendAsync("ReceiveMessage", message.Sender, message.Content);
                    }
                }
                else
                {
                    if (Context.UserIdentifier != message.Receiver)
                        await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", message.Sender, message.Content);
                }
            }
        }
    }
}
