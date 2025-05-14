using CodeFirst.Data;
using CodeFirst.Services;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        
        builder.Services.AddDbContext<DatabaseContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
        );
        
        builder.Services.AddScoped<IDbService, DbService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}