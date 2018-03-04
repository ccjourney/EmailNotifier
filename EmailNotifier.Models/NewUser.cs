using Newtonsoft.Json;

namespace EmailNotifier.Models
{
    public class NewUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "sentmail")]
        public bool SentMail { get; set; }
    }
}
