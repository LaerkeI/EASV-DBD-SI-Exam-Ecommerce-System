using Microsoft.EntityFrameworkCore;
using OrderManagementService.Data;
using OrderManagementService.Mappers;
using OrderManagementService.Messaging;
using OrderManagementService.Services;

namespace OrderManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register the DbContext with Dependency Injection (using SQL Server in this case)
            builder.Services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));
            
            // Register AutoMapper and your mapping profiles
            builder.Services.AddAutoMapper(typeof(Program));
            
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddSingleton<OrderEventProducer>();
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
