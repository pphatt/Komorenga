using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Komorenga.Models;
internal class MangaJSONModels
{
    public class MangaJSON
    {
        public List<Manga> data;
    }

    public class Manga
    {
        public string id
        {
            get; set;
        }

        public string type
        {
            get; set;
        }

        public List<MangaAttributes> attributes
        {
            get; set; 
        }
    }

    public class MangaAttributes
    {
        public Language title
        {
            get; set; 
        }
    }

    public class Language
    {
        public string en
        {
            get; set;
        }
    }
}
