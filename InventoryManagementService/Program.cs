using InventoryManagementService.Application.Interfaces;
using InventoryManagementService.Application.Services;
using InventoryManagementService.Infrastructure.Data;
using InventoryManagementService.Infrastructure.Messaging;
using InventoryManagementService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register the DbContext with Dependency Injection (using SQL Server in this case)
            builder.Services.AddDbContext<InventoryContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));

            // Register AutoMapper and your mapping profiles
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
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
