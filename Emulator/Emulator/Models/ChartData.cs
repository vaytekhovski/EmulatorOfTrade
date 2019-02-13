namespace Emulator
{
    using QuickType;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChartData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }

        [JsonProperty("open")]
        public double Open { get; set; }

        [JsonProperty("close")]
        public double Close { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("quoteVolume")]
        public double QuoteVolume { get; set; }

        [JsonProperty("weightedAverage")]
        public double WeightedAverage { get; set; }

    }
}
