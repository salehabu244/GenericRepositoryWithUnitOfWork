
using GenericRepositoryWithUnitOfWork.DAL;
using GenericRepositoryWithUnitOfWork.Repository;
using GenericRepositoryWithUnitOfWork.Repository.Interface;
using GenericRepositoryWithUnitOfWork.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryWithUnitOfWork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddDbContext<MyAppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
