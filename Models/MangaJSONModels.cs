﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.Models;
internal class MangaJSONModels
{
    public class MangaJSON
    {
        public string result { get; set; }
        public List<Manga> data;
    }

    public class MangaAttributes
    {
        public Language title
        {
            get; set; 
        }

        public Language parsedTitle
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
        public string fileName
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
