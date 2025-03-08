namespace SurveyBasket;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services
            .AddMapesterServices()
            .AddValidatorServices()
            .AddDependencyInsideServices();

        return services;
    }





    public static IServiceCollection AddMapesterServices(this IServiceCollection services)
    {

        //Add Mapster
        var mapppingConfig = TypeAdapterConfig.GlobalSettings;
        mapppingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mapppingConfig));
        

        

        return services;
    }
    public static IServiceCollection AddValidatorServices(this IServiceCollection services)
    {

        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMapster();

        return services;
    }

    public static IServiceCollection AddDependencyInsideServices(this IServiceCollection services)
    {
        services.AddScoped<IPollService, PollService>();

        return services;
    }

}
