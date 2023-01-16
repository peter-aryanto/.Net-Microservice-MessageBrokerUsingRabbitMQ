using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(configure =>
{
  configure.AddConsumers(Assembly.GetEntryAssembly());
  configure.UsingRabbitMq((context, configurator) =>
  { // context is equivalent to serviceProvider.
    configurator.Host("localhost");
    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("Provider", false));
    configurator.UseMessageRetry(retryConfigurator =>
    {
      retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
    });
  });
});
// builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
