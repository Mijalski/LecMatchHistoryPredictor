using System.Collections.Generic;

namespace LecMatchHistoryPredictor.Domain.Models
{
    public class Team
    {
        public string Name { get; set; }
        public string Side { get; set; }
        public List<string> Roster { get; set; } = new List<string>();
        public List<string> Picks { get; set; } = new List<string>();
    }
}