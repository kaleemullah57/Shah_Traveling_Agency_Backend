using Microsoft.AspNetCore.SignalR;

namespace Shah_Traveling_Agency_API.Areas.TicketHubArea.Models
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?
                .FindFirst("userID")?
                .Value;
        }
    }
}
