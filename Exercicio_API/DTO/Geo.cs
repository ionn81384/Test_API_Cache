using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Exercicio_API.DTO
{
    [Serializable]
    public class Geo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lng")]
        public string Lng { get; set; }
    }
}
