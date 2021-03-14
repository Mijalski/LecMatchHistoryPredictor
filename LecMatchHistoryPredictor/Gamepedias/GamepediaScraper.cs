using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LecMatchHistoryPredictor.Scraping.Models;
using LecMatchHistoryPredictor.Scraping.Savers;

namespace LecMatchHistoryPredictor.Scraping.Gamepedias
{
    public class GamepediaScraper : IGamepediaScraper
    {

        private readonly IMatchHistorySaver _matchHistorySaver;

        public GamepediaScraper(IMatchHistorySaver matchHistorySaver)
        {
            _matchHistorySaver = matchHistorySaver ?? throw new ArgumentNullException(nameof(matchHistorySaver));
        }


        private const string _url = "https://lol.gamepedia.com/Special:RunQuery/MatchHistoryGame?" +
                                    "pfRunQueryFormName=MatchHistoryGame&MHG[preload]=Tournament" +
                                    "&MHG[tournament]={0}&MHG[team]=&MHG[team1]=&MHG[team2]=&MHG[ban]=" +
                                    "&MHG[record]=&MHG[ascending][is_checkbox]=true&MHG[limit]=5000&MHG[offset]=" +
                                    "&MHG[where]=&MHG[textonly][is_checkbox]=true&MHG[textonly][value]=1" +
                                    "&wpRunQuery=Run query&pf_free_text=";

        private static readonly List<string> SeasonNames = new List<string>
        {
            "2021 Season", "2020 Season", "2019 Season", "2018 Season", "2017 Season", "2016 Season"
        };
        private static readonly List<string> SubSeasonNames = new List<string>
        {
            "Spring Season", "Spring Playoffs", "Summer Season", "Summer Playoffs"
        };

        public async Task GetMatchHistoryAsync()
        {
            var startTime = DateTime.UtcNow;
            var httpClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            httpClient.DefaultRequestHeaders.Accept.Clear();

            foreach (var season in SeasonNames)
            {
                foreach (var subSeason in SubSeasonNames)
                {
                    var seasonUrl = string.Format(_url, $"LEC/{season}/{subSeason}");

                    var response = await httpClient.GetStringAsync(seasonUrl);

                    var matches = ExtractMatchHistory(response);

                    await _matchHistorySaver.SaveMatchesAsync(matches,
                        $"lol-matches-scraped-{startTime:ddMMyyyyHHmm}.txt");
                }
            }
        }

        private IEnumerable<MatchHistory> ExtractMatchHistory(string htmlText)
        {
            var matches = new List<MatchHistory>();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlText);

            if (htmlDocument.DocumentNode.ChildNodes == null)
                return matches;

            foreach (var tableRow in htmlDocument.DocumentNode.SelectNodes("//tbody/tr[@class='multirow-highlighter']"))
            {
                var isBlueSideWin = tableRow.ChildNodes[2].InnerText == tableRow.ChildNodes[4].InnerText;

                var match = new MatchHistoryBuilder()
                    .Init()
                    .SetMatchDateTime(DateTime.Parse(tableRow.ChildNodes[0].InnerText))
                    .SetWinnerTeamName(tableRow.ChildNodes[isBlueSideWin ? 2 : 3].InnerText)
                    .SetLoserTeamName(tableRow.ChildNodes[isBlueSideWin ? 3 : 2].InnerText)
                    .SetWinnerTeamSide(isBlueSideWin ? "blue" : "red")
                    .SetLoserTeamSide(isBlueSideWin ? "red" : "blue")
                    .AddToWinnerTeamPicks(tableRow.ChildNodes[isBlueSideWin ? 7 : 8].InnerText.Split(','))
                    .AddToLoserTeamPicks(tableRow.ChildNodes[isBlueSideWin ? 8 : 7].InnerText.Split(','))
                    .AddToWinnerTeamRoster(tableRow.ChildNodes[isBlueSideWin ? 9 : 10].InnerText.Split(','))
                    .AddToLoserTeamRoster(tableRow.ChildNodes[isBlueSideWin ? 10 : 9].InnerText.Split(','));

                matches.Add(match.Build());
            }

            return matches;
        }
    }
}