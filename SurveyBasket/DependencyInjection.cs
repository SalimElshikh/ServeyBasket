using ApplicationLayer.Abstractions;
using ApplicationLayer.Authentication;
using ApplicationLayer.Reposatories;
using ApplicationLayer.Services;
using ApplicationLayer.Settings;
using DataLayer.Entities;
using DataLayer.Persistence;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;


namespace SurveyBasket;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddOpenApi();

        services.AddCors(options =>
            options.AddPolicy("AllowAll", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()

            )
        );


        services
            .AddMapsterConfig()
            .AddValidatorServices()
            .AddDependencyInsideServices(configuration)
            .AddDbContextConfig(configuration);

        services.AddAuthConfig(configuration);

        services.AddExceptionHandler<GlobalExeptionHandler>();
        services.AddProblemDetails();
        services.AddBackGroundConfig(configuration);
        services.AddDistributedMemoryCache();

        return services;
    }





    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }


    private static IServiceCollection AddValidatorServices(this IServiceCollection services)
    {

        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMapster();

        return services;
    }

    private static IServiceCollection AddDependencyInsideServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<ICachService, CacheService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        services.AddHttpContextAccessor();
        return services;
    }

    private static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var conncetionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection String 'DefaultConnection' Not found .");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(conncetionString));

        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();

        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();


        var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(op =>
        {
            op.SaveToken = true;
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings?.Key!)),
                ValidIssuer = settings?.Issuer,
                ValidAudience = settings?.Audience
            };
        });
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });
        return services;
    }

    // Add the processing server as IHostedService

    private static IServiceCollection AddBackGroundConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfireServer();
        services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        return services;
    }
}
