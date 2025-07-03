using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShowTime.Components;
using ShowTime.Context;
using ShowTime.Repositories.Implementations;
using ShowTime.Repositories.Interfaces;
using ShowTime.Services.Implementations;
using ShowTime.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Get ConnectionString from configuration

var connectionString = builder.Configuration.GetConnectionString("ShowTimeDbConnectionString") ??
    throw new InvalidOperationException("Connection string 'ShowTimeDbConnectionString' not found.");

//builder.Services.AddDbContext<ShowTimeDbContext>(options =>
//    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ShowTimeDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }));



// Register services and repositories
// Generic
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

// Festival
builder.Services.AddScoped<IFestivalRepository, FestivalRepository>();
builder.Services.AddScoped<IFestivalService, FestivalService>();

// Band
builder.Services.AddScoped<IBandRepository, BandRepository>();
builder.Services.AddScoped<IBandService, BandSerivce>();

// Booking

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

var app = builder.Build();

// Configure Blazorise and its providers.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
