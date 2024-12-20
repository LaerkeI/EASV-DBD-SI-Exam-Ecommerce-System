using InventoryManagementService.Application.Interfaces;
using InventoryManagementService.Application.Services;
using InventoryManagementService.Infrastructure.Messaging;
using Microsoft.Data.SqlClient;

namespace InventoryManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<SqlConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MSSQLConnection");
                return new SqlConnection(connectionString);
            });

            builder.Services.AddSingleton<IInventoryService, InventoryService>();
            builder.Services.AddSingleton<OrderEventConsumer>();
            builder.Services.AddHostedService<OrderEventConsumer>();
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
