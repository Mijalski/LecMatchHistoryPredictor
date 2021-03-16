using System;
using System.Collections.Generic;

namespace LecMatchHistoryPredictor.Domain.Models
{
    public class MatchHistoryBuilder
    {
        private MatchHistory _matchHistory = new MatchHistory();

        public MatchHistoryBuilder Init()
        {
            _matchHistory.WinnerTeam = new Team();
            _matchHistory.LoserTeam = new Team();
            return this;
        }

        public MatchHistory Build() => _matchHistory;

        public MatchHistoryBuilder SetMatchDateTime(DateTime dateTime)
        {
            _matchHistory.MatchDateTime = dateTime;
            return this;
        }

        public MatchHistoryBuilder SetWinnerTeamName(string teamName)
        {
            _matchHistory.WinnerTeam.Name = teamName;
            return this;
        }

        public MatchHistoryBuilder SetLoserTeamName(string teamName)
        {
            _matchHistory.LoserTeam.Name = teamName;
            return this;
        }

        public MatchHistoryBuilder SetWinnerTeamSide(string side)
        {
            _matchHistory.WinnerTeam.Side = side;
            return this;
        }

        public MatchHistoryBuilder SetLoserTeamSide(string side)
        {
            _matchHistory.LoserTeam.Side = side;
            return this;
        }

        public MatchHistoryBuilder AddToWinnerTeamRoster(IEnumerable<string> players)
        {
            if (_matchHistory.WinnerTeam.Roster.Count > 5)
            {
                throw new ArgumentException("Cannot add more members to roaster!");
            }

            _matchHistory.WinnerTeam.Roster.AddRange(players);
            return this;
        }

        public MatchHistoryBuilder AddToLoserTeamRoster(IEnumerable<string> players)
        {
            if (_matchHistory.LoserTeam.Roster.Count > 5)
            {
                throw new ArgumentException("Cannot add more members to roaster!");
            }

            _matchHistory.LoserTeam.Roster.AddRange(players);
            return this;
        }
        public MatchHistoryBuilder AddToWinnerTeamPicks(IEnumerable<string> champions)
        {
            if (_matchHistory.WinnerTeam.Picks.Count > 5)
            {
                throw new ArgumentException("Cannot add more champions to pick!");
            }

            _matchHistory.WinnerTeam.Picks.AddRange(champions);
            return this;
        }

        public MatchHistoryBuilder AddToLoserTeamPicks(IEnumerable<string> champions)
        {
            if (_matchHistory.LoserTeam.Picks.Count > 5)
            {
                throw new ArgumentException("Cannot add more champions to pick!");
            }

            _matchHistory.LoserTeam.Picks.AddRange(champions);
            return this;
        }

    }
}