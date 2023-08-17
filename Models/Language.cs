using Newtonsoft.Json;

namespace Komorenga.Models
{
    public class Language
    {
        public string en
        {
            get; set;
        }

        [JsonProperty(PropertyName = "ja-ro")]
        public string jaRo
        {
            get; set;
        }

        public string ja
        {
            get; set;
        }
    }
}
