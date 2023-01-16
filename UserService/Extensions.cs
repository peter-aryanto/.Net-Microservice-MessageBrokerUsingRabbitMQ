using MassTransit;
using System.Reflection;

namespace UserService
{
  public static class Extensions
  {
    public static IServiceCollection AddMassTransitUsingRabbitMq(this IServiceCollection services)
    {
      services.AddMassTransit(configure =>
      {
        configure.AddConsumers(Assembly.GetEntryAssembly());
        configure.UsingRabbitMq((context, configurator) =>
        { // context is equivalent to serviceProvider.
          configurator.Host("localhost");
          configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("User", false));
          configurator.UseMessageRetry(retryConfigurator =>
          {
            retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
          });
        });
      });
      // services.AddMassTransitHostedService();
      return services;
    }
  }
}