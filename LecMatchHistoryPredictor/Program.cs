using System;
using System.Threading.Tasks;
using LecMatchHistoryPredictor.Scraping.Gamepedias;
using LecMatchHistoryPredictor.Scraping.Savers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LecMatchHistoryPredictor.Scraping
{
    class Program
    {
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            var serviceProvider = services.BuildServiceProvider();

            var gamepediaScraper = serviceProvider.GetService<IGamepediaScraper>();
            await gamepediaScraper.GetMatchHistoryAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.AddConsole();
            });

            services.AddTransient<IGamepediaScraper, GamepediaScraper>()
                .AddTransient<IMatchHistorySaver, MatchHistorySaver>();
        }
    }
}
