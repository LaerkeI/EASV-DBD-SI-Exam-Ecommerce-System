using CatalogManagementService.Application.Services;
using CatalogManagementService.Infrastructure.Messaging;
using CatalogManagementService.Infrastructure.Repositories;
using MongoDB.Driver;

namespace CatalogManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var mongoConnectionString = configuration.GetConnectionString("MongoDBConnection");
                return new MongoClient(mongoConnectionString);
            });

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
            builder.Services.AddScoped<ICatalogService, CatalogService>();
            builder.Services.AddHostedService<OutOfStockEventConsumer>();
            builder.Services.AddControllers();
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
        }
    }
}
