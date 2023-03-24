using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Exercicio_API.DTO
{
    [Serializable]

    public class Company
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("catchPhrase")]
        public string CatchPhrase { get; set; }

        [JsonProperty("bs")]
        public string Bs { get; set; }
    }
}
