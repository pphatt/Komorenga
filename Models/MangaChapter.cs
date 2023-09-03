using System.Collections.Generic;

namespace Komorenga.Models;

public class MangaChapterResponse
{
    public string result
    {
        get; set;
    }

    public string response
    {
        get; set;
    }

    public List<MangaChapterVolume> data
    {
        get; set;
    }

    public string total
    {
        get; set;
    }
}

public class MangaChapterVolume
{
    public string id
    {
        get; set;
    }

    public string type
    {
        get; set;
    }

    public MangaChapterVolumeAttributes attributes
    {
        get; set;
    }

    public List<MangaChapterVolumeRelationship> relationships
    {
        get; set;
    }
}

public class MangaChapterVolumeAttributes
{
    public string volume
    {
        get; set;
    }

    public string chapter
    {
        get; set;
    }

    public string title
    {
        get; set;
    }

    public string publishAt
    {
        get; set;
    }

    public string pages
    {
        get; set;
    }
}

public class MangaChapterVolumeRelationship
{
    public string id
    {
        get; set;
    }

    public string type
    {
        get; set;
    }

    public MangaChapterVolumeRelationshipAttributes attributes
    {
        get; set;
    }
}

public class MangaChapterVolumeRelationshipAttributes
{
    public string name
    {
        get; set;
    }

    public string username
    {
        get; set;
    }
}