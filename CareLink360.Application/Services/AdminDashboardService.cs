using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Context;
using CareLink360.Domain.Enums; // 1. Added this using statement to access ContractStatus
using Microsoft.EntityFrameworkCore;

public class AdminDashboardService(ApplicationDbContext context) : IAdminDashboardService
{
    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        var totalCaregivers = await context.Caregivers.CountAsync();

        var totalClients = await context.Clients.CountAsync();

        var pendingNin = await context.NinVerifications
            .CountAsync(x => x.Status == "Pending");

        var activeContracts = await context.Contracts
            .CountAsync(x => x.Status == ContractStatus.Successful);

        var completedPayments = await context.Payments
            .CountAsync(x => x.Status == "Paid");

        var avgRating = await context.Caregivers.AnyAsync()
            ? await context.Caregivers.AverageAsync(x => x.AverageRating)
            : 0;

        return new AdminDashboardDto
        {
            TotalCaregivers = totalCaregivers,
            TotalClients = totalClients,
            PendingNinVerifications = pendingNin,
            ActiveContracts = activeContracts,
            CompletedPayments = completedPayments,
            AverageCaregiverRating = avgRating
        };
    }
}
