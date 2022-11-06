using Newtonsoft.Json;

namespace WebApplication2.Models.Travel
{
    public struct Bearings
    {
        [JsonProperty]
        public double average { get; set; }

        [JsonProperty]
        public double minimum { get; set; }

        [JsonProperty]
        public double maximum { get; set; }
    }
}