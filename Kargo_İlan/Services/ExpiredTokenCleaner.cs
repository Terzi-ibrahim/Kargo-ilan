using Kargo_İlan.Data;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Services
{
    public class ExpiredTokenCleaner : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpiredTokenCleaner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _context = scope.ServiceProvider.GetRequiredService<KargoDbContext>();

                        // Süresi dolmuş reset tokenları temizle
                        var expiredUserTokens = await _context.User
                            .Where(u => u.ResetTokenExpiry.HasValue && u.ResetTokenExpiry < DateTime.UtcNow)
                            .ToListAsync(stoppingToken);

                        foreach (var user in expiredUserTokens)
                        {
                            user.ResetToken = null;
                            user.ResetTokenExpiry = null;
                        }

                        await _context.SaveChangesAsync(stoppingToken);
                    }

                    // 10 dakikada bir çalışacak
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                // Uygulama kapanırken task iptal edildiyse bu normaldir, loglamaya gerek yok
            }
            catch (Exception ex)
            {
                // Gerçek bir hata oluşursa loglayabilirsin
                Console.WriteLine($"[ExpiredTokenCleaner] Hata: {ex.Message}");
            }
        }
    }
}
