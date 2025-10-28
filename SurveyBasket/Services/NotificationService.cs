namespace SurveyBasket.Services;

public class NotificationService(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IEmailSender emailSender) : INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task SendNewPollsNotification(int? id)
    {
        IEnumerable<Poll> polls = [];

        if(id.HasValue)
        {
            var poll = await _context.Polls.SingleOrDefaultAsync(p => p.Id == id && p.IsPublished);
        }
        else
        {
            polls = await _context.Polls
                .Where(p => p.IsPublished && p.StartsAt == DateOnly.FromDateTime(DateTime.Now))
                .AsNoTracking()
                .ToListAsync();
        }
        
        // TODO: Members Only
        var users = await _userManager.Users.ToListAsync();

        //var origin = _httpContextAccessor.HttpContext!.Request.Headers.Origin; {origin}/polls/start/{poll.Id}

        foreach (var poll in polls)
        {
            foreach(var user in users)
            {
                var placeholders = new Dictionary<string, string>
                {
                    {"{{name}}", user.FirstName },
                    {"{{pollTill}}", poll.Title},
                    {"{{endDate}}", poll.EndsAt.ToString()},
                    {"{{url}}", $"https://e-commerce-iti-six.vercel.app/"},
                };

                var emailBody = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholders);

                BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                    user.Email!,
                    $"🚨 Survey Basket - New Poll: {poll.Title}",
                    emailBody
                ));
                await Task.CompletedTask;
            }
        }
    }
}
