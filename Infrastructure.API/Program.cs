using Application;
using Application.Interfaces;
using Infrastructure.Data.Adapters;
using Infrastructure.Data.Context;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PeopleDevSofkaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PeopleDevSofkaContext") ?? throw new InvalidOperationException("Connection string 'PeopleDevSofkaContext' not found.")));

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SofkaStatisticsDbContext>();

builder.Services.AddSingleton<IServiceQueueBus, ServiceQueueBusSofkerStatistics>();
builder.Services.AddSingleton<IAdapterSofkerStatistic, AdapterSofkerStatistic>();

builder.Services.AddSingleton(_ => builder.Configuration);

builder.Services.AddHostedService<WorkerQueueBusSofkerStatistics>();

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

app.UseCors(builder => builder
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin());

app.Run();
