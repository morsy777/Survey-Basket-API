using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Health;
using System.Threading.RateLimiting;

namespace SurveyBasket;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Add CORS Policy

        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(allowedOrigins!)
            )
        );


        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));

        services.AddControllers();

        services.AddSwaggerServices();
        
        services.AddMapsterConfig();

        services.AddFluentValidationConfig();

        services.AddAuthConfig(configuration);
        services.AddBackgroundJobsConfig(configuration);

        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IRoleService, RoleService>();

        // Mail Settings
        services.AddScoped<IEmailSender, EmailService>();
        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        services.AddHttpContextAccessor();

        // Exception Handler
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Caching
        //services.AddScoped<ICacheService, CacheService>();
        services.AddHybridCache();

        // Health Check
        services.AddHealthChecks()
            .AddSqlServer(name: "database", connectionString: connectionString)
            .AddHangfire(options =>
            {
                options.MinimumAvailableServers = 1;
            })
            .AddCheck<MailProviderHealthCheck>(name: "mail service");

        // Rate limiting
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.AddPolicy("ipLimit", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );

            rateLimiterOptions.AddPolicy("userLimit", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.GetUserId(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );

            rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
            {
                options.PermitLimit = 1000;
                options.QueueLimit = 100;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            //rateLimiterOptions.AddTokenBucketLimiter("token", options =>
            //{
            //    options.TokenLimit = 2;
            //    options.QueueLimit = 1;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
            //    options.TokensPerPeriod = 2;
            //    options.AutoReplenishment = true;
            //});

            //rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
            //{
            //    options.PermitLimit = 2;
            //    options.Window = TimeSpan.FromSeconds(20);
            //    options.QueueLimit = 1;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //});
        });

        return services;
    }

    private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }

    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddIdentity<ApplicationUser, IdentityRole>()
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Role-Based Authentication
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        return services;
    }

    private static IServiceCollection AddBackgroundJobsConfig(this  IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"))
        );

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }
}
