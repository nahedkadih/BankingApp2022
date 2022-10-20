using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using BankApp.Data;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MessagesAPI.ExtensionMethods;

public static class ServicesExtensions
{ 
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    { 
        var serviceProvider = services.BuildServiceProvider(); 
        var context = serviceProvider.GetService<BankContext>();

        if (!context.Users.Any(s => s.userId == "User_1"))
        {
            var users = new List<User>
                {
                   new User
                   {
                       userId = "User_1",// for test only  Guid.NewGuid().ToString("N").ToUpper(),
                       name = "Nader Ssam",
                       date_created = DateTime.Now
                   }
                };
            context.AddRange(users);
            context.SaveChangesAsync();
        }

        return services;
    }
    
}

public class BankAppDataInitializer
{
    public async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetService<BankContext>();
        var databaseCreated = await db.Database.EnsureCreatedAsync();
        if (databaseCreated)
        {
            await GenerateData(db);
        }
    }

    private static async Task GenerateData(BankContext context)
    {
        if (!context.Users.Any(s => s.userId == "User_4"))
        {
            var users = new List<User>
                {
                   new User
                   {
                       userId = "User_4",// for test only  Guid.NewGuid().ToString("N").ToUpper(),
                       name = "Nader Ssam",
                       date_created = DateTime.Now
                   }
                };
            context.AddRange(users);
           await  context.SaveChangesAsync();
        }

        // or execute custom sql here
    }
}

