var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddDistributedMemoryCache();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseHangfireDashboard();

app.UseHttpsRedirection();

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
    DashboardTitle = "Survery Basket Dashboard",
    //IsReadOnlyFunc = (DashboardContext context) => true
});

RecurringJob.AddOrUpdate<INotificationService>(
    "Daily-notification-job",
    x => x.SendNewPollsNotification(null),
    Cron.Daily(17, 30)
);

app.UseSerilogRequestLogging();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run(); 
