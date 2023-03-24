using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Exercicio_API.DTO
{
    [Serializable]

    public class Address
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("suite")]
        public string Suite { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }

        [JsonProperty("geo")]
        public Geo Geo { get; set; }
    }
}
