using ApplicationLayer.Reposatories;
using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Routing;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddDependencies(builder.Configuration);
// Serilog for logging 
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();

app.UseCors();
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
    [

        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
    ],
    DashboardTitle = "Survay Basket Jobs",
    //IsReadOnlyFunc = (DashboardContext context) => true


});
var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopedFactory.CreateScope();
var nogificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

RecurringJob.AddOrUpdate("SendNewPollsNotificationsd", () => nogificationService.SendNewPollsNotifications(null), Cron.Daily);
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();

app.Run();
