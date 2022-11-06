using Newtonsoft.Json;

namespace WebApplication2.Models.Travel
{
    public struct MetaData
    {
        [JsonProperty]
        public Bearings bearingLimits { get; set; }
    }
}