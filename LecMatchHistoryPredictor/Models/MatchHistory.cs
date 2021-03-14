using System;

namespace LecMatchHistoryPredictor.Scraping.Models
{
    public class MatchHistory
    {
        public Team WinnerTeam { get; set; }
        public Team LoserTeam { get; set; }
        public DateTime MatchDateTime { get; set; }
    }
}