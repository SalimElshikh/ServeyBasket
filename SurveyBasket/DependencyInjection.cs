using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Authentication;
using System.Text;

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
            .AddMapesterServices()
            .AddValidatorServices()
            .AddDependencyInsideServices()
            .AddDbContextConfig(configuration);

        services.AddAuthConfig(configuration);

        services.AddExceptionHandler<GlobalExeptionHandler>();
        services.AddProblemDetails();

        return services;
    }





    private static IServiceCollection AddMapesterServices(this IServiceCollection services)
    {

        //Add Mapster
        var mapppingConfig = TypeAdapterConfig.GlobalSettings;
        mapppingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mapppingConfig));
        

        

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

    private static IServiceCollection AddDependencyInsideServices(this IServiceCollection services)
    {
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IResultService, ResultService>();

        return services;
    }

    private static IServiceCollection AddDbContextConfig(this IServiceCollection services , IConfiguration configuration)
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
            .AddEntityFrameworkStores<ApplicationDbContext>();

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
        return services;
    }

}
