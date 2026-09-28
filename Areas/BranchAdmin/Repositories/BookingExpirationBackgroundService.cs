using Microsoft.AspNetCore.SignalR;
using Shah_Traveling_Agency_API.Areas.PublicArea.Repositories;
using Shah_Traveling_Agency_API.Areas.TicketHubArea.Models;

namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Repositories
{
    public class BookingExpirationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<TicketHub> _ticketHub;
        private readonly ILogger<BookingExpirationBackgroundService> _logger;

        public BookingExpirationBackgroundService(
            IServiceScopeFactory scopeFactory,
            IHubContext<TicketHub> ticketHub,
            ILogger<BookingExpirationBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _ticketHub = ticketHub;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Booking expiration background service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =
                        _scopeFactory.CreateScope();

                    var bookingRepo = scope.ServiceProvider.GetRequiredService<BranchAdminRepo>();

                    var expiredBookings =
                        await bookingRepo.ExpireHeldBookingsAsync();

                    if (expiredBookings != null &&
                        expiredBookings.Count > 0)
                    {
                        foreach (var booking in expiredBookings)
                        {
                            await _ticketHub.Clients.All.SendAsync(
                                "TicketInventoryUpdated",
                                new
                                {
                                    purchaseInvoiceItemId =
                                        booking.PurchaseInvoiceItemId
                                },
                                stoppingToken);

                            _logger.LogInformation(
                                "Booking expired automatically. " +
                                "BookingId: {BookingId}, " +
                                "PurchaseInvoiceItemId: {PurchaseInvoiceItemId}, " +
                                "Quantity: {Quantity}",
                                booking.BookingId,
                                booking.PurchaseInvoiceItemId,
                                booking.Quantity);
                        }
                    }
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occurred while expiring held bookings.");
                }

                try
                {
                    await Task.Delay(
                        TimeSpan.FromMinutes(1),
                        stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            _logger.LogInformation(
                "Booking expiration background service stopped.");
        }
    }
}