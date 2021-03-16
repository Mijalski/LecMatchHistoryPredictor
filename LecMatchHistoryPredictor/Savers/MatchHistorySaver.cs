using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LecMatchHistoryPredictor.Domain.Models;

namespace LecMatchHistoryPredictor.Scraping.Savers
{
    public interface IMatchHistorySaver
    {
        Task SaveMatchesAsync(IEnumerable<MatchHistory> matches, string fileName);
    }

    public class MatchHistorySaver : IMatchHistorySaver
    {
        public async Task SaveMatchesAsync(IEnumerable<MatchHistory> matches, string fileName)
        {
            var lines = matches.Select(_ =>
                $"{_.MatchDateTime};{_.WinnerTeam.Name};{_.LoserTeam.Name};{_.WinnerTeam.Side};{_.LoserTeam.Side}" +
                $";{string.Join(',', _.WinnerTeam.Roster)};{string.Join(',', _.LoserTeam.Roster)}" +
                $";{string.Join(',', _.LoserTeam.Picks)};{string.Join(',', _.LoserTeam.Picks)}");

            if (File.Exists(fileName))
            {
                await File.AppendAllLinesAsync(fileName, lines);
            }
            else
            {
                await File.WriteAllLinesAsync(fileName, lines);
            }
        }
    }
}