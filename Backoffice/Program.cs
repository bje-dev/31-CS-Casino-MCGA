using RabbitMqService.RabbitMq;
using RabbitMqServiceMCGA.Queues;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMq(settings =>
{
    settings.ConnectionString = builder.Configuration.GetValue<string>("RabbitMq:ConnectionString");
    settings.ExchangeName = builder.Configuration.GetValue<string>("AppSettings:ApplicationName");
    settings.QueuePrefetchCount = builder.Configuration.GetValue<ushort>("AppSettings:QueuePrefetchCount");
}, queues =>
{
    //Agregamos colas para las apis...
    queues.Add<Pagos>();
    queues.Add<Depositos>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();

