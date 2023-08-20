using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komorenga.Models;
public class MangaChapterImage
{
    public string result
    {
        get; set;
    }

    public MangaChapterImageHashCode chapter
    {
        get; set;
    }
}

public class MangaChapterImageHashCode
{
    public string hash
    {
        get; set;
    }

    public List<string> data
    {
        get; set; 
    }

    public List<string> dataSaver
    {
        get; set; 
    }
}

public class MangaChapterImageUrl
{
    public string url
    {
        get; set; 
    }
}
