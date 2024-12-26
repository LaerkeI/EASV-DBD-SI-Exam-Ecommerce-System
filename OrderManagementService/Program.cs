using Microsoft.EntityFrameworkCore;
using OrderManagementService.Application.Interfaces;
using OrderManagementService.Application.Services;
using OrderManagementService.Infrastructure.Data;
using OrderManagementService.Infrastructure.Messaging;
using OrderManagementService.Infrastructure.Repositories;

namespace OrderManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register the DbContext with Dependency Injection (using SQL Server)
            builder.Services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));
            
            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));
            
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddSingleton<CreatedOrderEventProducer>();
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
