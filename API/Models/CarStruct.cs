using Newtonsoft.Json;

namespace WebApplication2.Models
{
    public struct CarStruct
    {
        [JsonProperty]
        public double FuelConsumption { get; set; }
        [JsonProperty]
        public string? Make { get; set; }
        [JsonProperty]
        public string RegistrationNumber { get; set; }
    }
}
