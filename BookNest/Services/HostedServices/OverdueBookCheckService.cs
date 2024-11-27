using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookNest.Services.HostedServices
{
    public class OverdueBookCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OverdueBookCheckService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Check every 24 hours

        public OverdueBookCheckService(IServiceProvider serviceProvider, ILogger<OverdueBookCheckService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for overdue books...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
                    userService.CheckAndRestrictUsers();
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
