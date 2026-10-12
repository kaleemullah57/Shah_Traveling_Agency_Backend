
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Shah_Traveling_Agency_API.Areas.TicketHubArea.Models
{
    [Authorize]
    public class TicketHub : Hub
    {
        public static string CustomerLedgerGroup(int customerId)
            => $"ledger-customer-{customerId}";

        public async Task JoinCustomerLedger(int customerId)
        {
            var user = Context.User;

            var userIdValue =
                user?.FindFirst("userID")?.Value ??
                user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userTypeValue =
                user?.FindFirst("userTypeId")?.Value ??
                user?.FindFirst("UserTypeId")?.Value;

            if (!int.TryParse(userIdValue, out var userId) ||
                !int.TryParse(userTypeValue, out var userTypeId))
            {
                throw new HubException("Invalid authenticated user.");
            }

            if (customerId <= 0)
                throw new HubException("Invalid customer.");

            // Customer can join only their own ledger.
            if (userTypeId == 3 && userId != customerId)
                throw new HubException(
                    "You cannot access another customer's ledger.");

            if (userTypeId is not (1 or 2 or 3))
                throw new HubException("Access denied.");

            // IMPORTANT:
            // Before enabling this for branch admins, verify that
            // the customer belongs to their authorized branch.
            if (userTypeId == 2)
                throw new HubException(
                    "Branch authorization must be verified first.");

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                CustomerLedgerGroup(customerId));
        }
    }
}
