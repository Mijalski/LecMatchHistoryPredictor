using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using LecMatchHistoryPredictor.Domain.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace LecMatchHistoryPredictor.Predictor
{
    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromTextFile<MatchHistory>
                ("lol-matches-scraped-150320211906.txt", separatorChar: ';', hasHeader: false);
        }
    }
}
