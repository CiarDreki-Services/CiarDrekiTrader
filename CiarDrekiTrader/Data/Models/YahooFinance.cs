using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiarDrekiTrader.Data.Models
{
    public class YahooFinance
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Chart
    {
        [JsonProperty("result")]
        public List<Result> Result { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }

    public class CurrentTradingPeriod
    {
        [JsonProperty("pre")]
        public Pre Pre { get; set; }

        [JsonProperty("regular")]
        public Regular Regular { get; set; }

        [JsonProperty("post")]
        public Post Post { get; set; }
    }

    public class Indicators
    {
        [JsonProperty("quote")]
        public List<Quote> Quote { get; set; }
    }

    public class Meta
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("instrumentType")]
        public string InstrumentType { get; set; }

        [JsonProperty("firstTradeDate")]
        public int? FirstTradeDate { get; set; }

        [JsonProperty("regularMarketTime")]
        public int RegularMarketTime { get; set; }

        [JsonProperty("gmtoffset")]
        public int Gmtoffset { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("exchangeTimezoneName")]
        public string ExchangeTimezoneName { get; set; }

        [JsonProperty("regularMarketPrice")]
        public double RegularMarketPrice { get; set; }

        [JsonProperty("chartPreviousClose")]
        public double ChartPreviousClose { get; set; }

        [JsonProperty("previousClose")]
        public double PreviousClose { get; set; }

        [JsonProperty("scale")]
        public int Scale { get; set; }

        [JsonProperty("priceHint")]
        public int PriceHint { get; set; }

        [JsonProperty("currentTradingPeriod")]
        public CurrentTradingPeriod CurrentTradingPeriod { get; set; }

        [JsonProperty("tradingPeriods")]
        public List<List<object>> TradingPeriods { get; set; }

        [JsonProperty("dataGranularity")]
        public string DataGranularity { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("validRanges")]
        public List<string> ValidRanges { get; set; }
    }

    public class Post
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("gmtoffset")]
        public int Gmtoffset { get; set; }
    }

    public class Pre
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("gmtoffset")]
        public int Gmtoffset { get; set; }
    }

    public class Quote
    {
        [JsonProperty("low")]
        public List<decimal?> Low { get; set; }

        [JsonProperty("open")]
        public List<decimal?> Open { get; set; }

        [JsonProperty("close")]
        public List<decimal?> Close { get; set; }

        [JsonProperty("volume")]
        public List<int?> Volume { get; set; }

        [JsonProperty("high")]
        public List<decimal?> High { get; set; }
    }

    public class Regular
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("gmtoffset")]
        public int Gmtoffset { get; set; }
    }

    public class Result
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("timestamp")]
        public List<int> Timestamp { get; set; }

        [JsonProperty("indicators")]
        public Indicators Indicators { get; set; }
    }

    public class Root
    {
        [JsonProperty("chart")]
        public Chart Chart { get; set; }
    }


}
