namespace Infrastructure.JsonDAL.DAL
{
    using EducationPortal.Infrastructure.JsonDAL.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class DependencyRegistratorJsonDAL
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISerializer, JsonSerializer>()
                    .AddTransient<JsonFormatter>()
                    .AddTransient<JsonReaderAndWriter>();
        }
    }
}
