using Api.Receiver;
using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.AppSettings;
using Domain.RabbitMessages;
using RabbitMqService.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDepositosService, DepositosService>();
builder.Services.AddTransient<IDepositosRepository, DepositosRepository>();

builder.Services.AddSingleton<ConnectionStrings>(sp =>
{
    var connstring = new ConnectionStrings();
    connstring.SqlConnection = builder.Configuration.GetConnectionString("SqlConnection");
    return connstring;
});

builder.Services.AddRabbitMq(settings =>
{
    settings.ConnectionString = builder.Configuration.GetValue<string>("RabbitMq:ConnectionString");
    settings.ExchangeName = builder.Configuration.GetValue<string>("AppSettings:ApplicationName");
    settings.QueuePrefetchCount = builder.Configuration.GetValue<ushort>("AppSettings:QueuePrefetchCount");
}, queues =>
{
    //Agregamos colas para las apis...
    queues.Add("Depositos");
})
.AddReceiver<DepositosReceiver<DepositosMessage>, DepositosMessage, DepositosService>();

builder.Services.AddSingleton<DepositosReceiver<DepositosMessage>>();

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
