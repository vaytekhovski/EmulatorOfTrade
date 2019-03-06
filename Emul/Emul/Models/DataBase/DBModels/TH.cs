using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emul.Models.DataBase.DBModels
{
    public class TH
    {
        public int Id { get; set; }
        public int EmulationNumber { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Type { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double Total { get; set; }
        public double Fee { get; set; }
        public double Balance { get; set; }
    }
}