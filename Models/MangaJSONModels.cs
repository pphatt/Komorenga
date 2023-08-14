using System.Collections.Generic;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.Models;
internal class MangaJSONModels
{
    public class MangaJSON
    {
        public string result
        {
            get; set;
        }
        public List<Manga> data;
    }

    public class MangaAttributes
    {
        public Language title
        {
            get; set;
        }

        public List<Language> altTitles
        {
            get; set;
        }

        public string status
        {
            get; set;
        }

        public string year
        {
            get; set;
        }

        public string contentRating
        {
            get; set;
        }
    }

    public class MangaRelationships
    {
        public string id
        {
            get; set;
        }

        public string type
        {
            get; set;
        }

        public MangaRelationshipsAttributes attributes
        {
            get; set;
        }
    }

    public class MangaRelationshipsAttributes
    {
        public string name
        {
            get; set;
        }

        public string fileName
        {
            get; set;
        }
    }
}

internal class Manga
{
    public string id
    {
        get; set;
    }

    public string type
    {
        get; set;
    }

    public string author
    {
        get; set;
    }

    public string poster
    {
        get; set;
    }

    public MangaAttributes attributes
    {
        get; set;
    }

    public List<MangaRelationships> relationships
    {
        get; set;
    }
}
