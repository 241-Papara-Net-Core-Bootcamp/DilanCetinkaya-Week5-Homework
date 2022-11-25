using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Core.Configuraitons;

using User.Core.Interfaces;
using User.Infrastructure.Interfaces;
using User.Infrastructure.Repositories;
using User.Infrastructure;
using User.Application.Services;
using User.Application.Caching;
using User.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using User.Application.Map;

namespace User.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    
                    services.AddHostedService<Worker>();
                    services.AddDbContext<UserDbContext>(options =>
                    options.UseSqlServer(
                        hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddAutoMapper(typeof(MapProfile));
                    services.AddHangfireServer();
                    services.AddHangfire(x => x.UseSqlServerStorage(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    services.Configure<CacheConfiguration>(hostContext.Configuration.GetSection("CacheConfiguration"));

                    services.AddMemoryCache();
                    services.AddTransient<ICacheService, MemoryCacheService>();

                });
    }
}
