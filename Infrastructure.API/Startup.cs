using Application.Interfaces;
using Application;
using Infrastructure.Data.Adapters;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace Infrastructure.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configurationSection = Configuration.GetConnectionString("PeopleDevSofkaContext");
            services.AddDbContext<PeopleDevSofkaContext>(options => options.UseSqlServer(configurationSection));

            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSingleton<SofkaStatisticsDbContext>();

            services.AddSingleton<IServiceQueueBus, ServiceQueueBusSofkerStatistics>();
            services.AddSingleton<IAdapterSofkerStatistic, AdapterSofkerStatistic>();

            services.AddSingleton(_ => Configuration);

            services.AddHostedService<WorkerQueueBusSofkerStatistics>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();

            app.UseCors(builder => builder
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/version", async context =>
                {
                    await context.Response.WriteAsync(Environment.GetEnvironmentVariable("VERSION") ?? "0.0.0");
                });
                endpoints.MapControllers();
            });
        }
    }
}
