using System;
using System.Threading.Tasks;
using LecMatchHistoryPredictor.Scraping.Gamepedias;
using LecMatchHistoryPredictor.Scraping.Savers;
using Microsoft.Extensions.DependencyInjection;

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
            //setup DI
            var serviceProvider = new ServiceCollection()
                .AddTransient<IGamepediaScraper, GamepediaScraper>()
                .AddTransient<IMatchHistorySaver, MatchHistorySaver>()
                .BuildServiceProvider();

            var gamepediaScraper = serviceProvider.GetService<IGamepediaScraper>();
            await gamepediaScraper.GetMatchHistoryAsync();
        }
    }
}
