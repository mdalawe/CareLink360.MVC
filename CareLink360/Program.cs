using CareLink360.Application.Services;
using CareLink360.Application.Interfaces;
using CareLink360.Context;
using CareLink360.Domain.Interface;
using CareLink360.Infrastructure.Repository;
using CareLink360.Web.DbInitializer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));




builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";

        options.AccessDeniedPath = "/Home/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

        options.SlidingExpiration = true;
    });



builder.Services.AddAuthorization();



builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ICaregiverService, CaregiverService>();

builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddScoped<IJobRequestService, JobRequestService>();

builder.Services.AddScoped<IJobTypeService, JobTypeService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();


// =====================================================
// REPOSITORIES
// =====================================================

builder.Services.AddScoped<ICaregiverRepository, CaregiverRepository>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddScoped<IContractRepository, ContractRepository>();

builder.Services.AddScoped<IJobRequestRepository, JobRequestRepository>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IRatingRepository, RatingRepository>();


// =====================================================
// BUILD APPLICATION
// =====================================================

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await DbInitializer.SeedAdminAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogError(
            ex,
            "An error occurred while seeding the database.");
    }
}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();