﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuickType
{
    public partial class TradeHistory
    {
        [JsonProperty("globalTradeID")]
        public long GlobalTradeId { get; set; }

        [JsonProperty("tradeID")]
        public long TradeId { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }
    }

    public enum TypeEnum { Buy, Sell };

    public partial class TradeHistory
    {
        public static List<TradeHistory> FromJson(string json) => JsonConvert.DeserializeObject<List<TradeHistory>>(json, Converter.Settings);
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "buy":
                    return TypeEnum.Buy;
                case "sell":
                    return TypeEnum.Sell;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Buy:
                    serializer.Serialize(writer, "buy");
                    return;
                case TypeEnum.Sell:
                    serializer.Serialize(writer, "sell");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
