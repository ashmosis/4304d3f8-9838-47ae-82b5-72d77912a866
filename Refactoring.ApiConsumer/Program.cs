namespace Refactoring.ApiConsumer
{
    using MassTransit;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Consumers;
    using Models;
    using NoChangeInfrastructure;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureMassTransit(services);

                    var config = hostContext.Configuration;
                    services.Configure<ApiConfig>(config.GetSection("ApiConfig"));

                    services.AddSingleton<IFakeFeed, FakeFeedProxy>();
                    services.AddHostedService<EnqueueCandidatesWorker>();
                });

        private static void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(serviceConfig =>
            {
                serviceConfig.SetKebabCaseEndpointNameFormatter();
                serviceConfig.AddConsumersFromNamespaceContaining<RegisterNewCandidateConsumer>();
                serviceConfig.UsingInMemory((ctx, cfg) =>
                {
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddMassTransitHostedService(true);
        }
    }
}
