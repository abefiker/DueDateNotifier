namespace DueDateNotifier.Services
{
    public class DueDateNotifierService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public DueDateNotifierService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<YourDbContext>();

                // Query the database to find tasks with due dates that have arrived
                var tasksWithDueDate = dbContext.Tasks
                    .Where(t => t.DueDate <= DateTime.Now && !t.Notified)
                    .ToList();

                foreach (var task in tasksWithDueDate)
                {
                    // TODO: Implement local push notification logic here
                    // You may use a library like ToastNotifications or other suitable options
                    // Update task as notified to avoid sending duplicate notifications
                    task.Notified = true;
                }

                dbContext.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
