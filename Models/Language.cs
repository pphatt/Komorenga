using Newtonsoft.Json;

namespace Komorenga.Models
{
    class Language
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
    }
}
