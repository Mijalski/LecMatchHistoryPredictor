using System.Threading.Tasks;

namespace LecMatchHistoryPredictor.Scraping.Gamepedias
{
    public interface IGamepediaScraper
    {
        Task GetMatchHistoryAsync();
    }
}