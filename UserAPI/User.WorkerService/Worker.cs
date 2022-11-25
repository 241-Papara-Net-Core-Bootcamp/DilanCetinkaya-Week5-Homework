using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using User.Infrastructure.Dtos;
using User.Infrastructure.Interfaces;

namespace User.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<BackgroundWorker> logger;

        public Worker(
            IServiceScopeFactory scopeFactory,
            ILogger<BackgroundWorker> logger)
        {

            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("{Type} is now running in the background", nameof(BackgroundWorker));
            await BackgroundProcessing(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogCritical("The {Type} is stopping due to host shutdown, " +
                "queued item might not processed anymore", nameof(BackgroundWorker));
            return base.StopAsync(cancellationToken);
        }

        private async Task BackgroundProcessing(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("deneme");
                    await Task.Delay(5000, cancellationToken);
                    using var httpClient = new HttpClient();


                    var result = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");

                    var jsonString = await result.Content.ReadAsStringAsync();

                    var users = JsonSerializer.Deserialize<List<UserDto>>(jsonString);

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                        foreach (var item in users)
                        {
                            await userService.AddAsync(item);
                        }
                    }

                }
                catch (Exception ex)
                {
                    logger.LogCritical("An error occured when publishing a book exception {Exception}", ex);
                }
            }
        }
    }
}


