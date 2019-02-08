namespace QuickType
{
    using System.Collections.Generic;
    using Newtonsoft.Json;


    public partial class ChartData
    {
        public int Id { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

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

    public partial class ChartData
    {
        public static List<ChartData> FromJson(string json) => JsonConvert.DeserializeObject<List<ChartData>>(json, QuickType.Converter.Settings);
    }
}
