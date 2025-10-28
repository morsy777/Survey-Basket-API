var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

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
    DashboardTitle = "Survery Basket Dashboard"
});

RecurringJob.AddOrUpdate<INotificationService>(
    "weekly-notification-job",
    x => x.SendNewPollsNotification(null),
    Cron.Minutely()
);

BackgroundJob.Enqueue<INotificationService>(
    x => x.SendNewPollsNotification(null)
);

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run(); 
