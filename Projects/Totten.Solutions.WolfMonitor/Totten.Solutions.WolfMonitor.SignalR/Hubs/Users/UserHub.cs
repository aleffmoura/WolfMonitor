using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.SignalR.Hubs.Users
{
    public class UserHub : Hub
    {
        public static readonly ConcurrentDictionary<string, UserConnected> HubConnectedUsers = new ConcurrentDictionary<string, UserConnected>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var userId = GetUserIdFromRequest();

            HubConnectedUsers.AddOrUpdate(userId,
                new UserConnected { Id = userId, ConnectIds = new List<string> { Context.ConnectionId } },
                (unnecessaryKey, oldConnectedNotification) =>
                {
                    oldConnectedNotification.ConnectIds.Add(Context.ConnectionId);

                    oldConnectedNotification.ConnectIds.Distinct();

                    return oldConnectedNotification;
                });
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);

            var userId = GetUserIdFromRequest();

            UserConnected connectedAgent;
            if (HubConnectedUsers.TryGetValue(userId, out connectedAgent))
            {
                connectedAgent.ConnectIds.Remove(Context.ConnectionId);

                if (connectedAgent.ConnectIds.Count == 0)
                {
                    HubConnectedUsers.TryRemove(userId, out _);
                }
            }
        }

        private string GetUserIdFromRequest()
        {
            var httpContext = Context.GetHttpContext();

            return httpContext.Request.Query["UserId"].ToString();
        }
    }
}
