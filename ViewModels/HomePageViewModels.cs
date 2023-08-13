using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Komorenga.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.WindowsAppSDK.Runtime;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Media;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.ViewModels;

internal class HomePageViewModels
{
    public ObservableCollection<Manga> Manga { get; } = new ObservableCollection<Manga>();

    public HomePageViewModels()
    {
        DateTime utcTime = DateTime.UtcNow.AddMonths(-1);
        var encodedUtcTime = HttpUtility.UrlEncode(utcTime.ToString("yyyy-MM-ddTHH:mm:ss")).ToUpper();

        FetchData($"https://api.mangadex.org/manga?includes[]=cover_art&includes[]=artist&includes[]=author&order[followedCount]=desc&contentRating[]=safe&contentRating[]=suggestive&hasAvailableChapters=true&createdAtSince={encodedUtcTime}");
    }

    private async Task FetchData(string url)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    MangaJSON manga = JsonConvert.DeserializeObject<MangaJSON>(responseData);

                    for (int i = 0; i < manga.data.Count; i++)
                    {
                        var getMangaPoster = "";
                        var getMangaAuthor = "";

                        System.Diagnostics.Debug.WriteLine(manga.data[i].relationships.Count);

                        for (var j = 0; j < manga.data[i].relationships.Count; j++)
                        {
                            switch (manga.data[i].relationships[j].type)
                            {
                                case "author":
                                    if (getMangaAuthor.Length > 0)
                                    {
                                        getMangaAuthor += ", ";
                                    }

                                    getMangaAuthor += manga.data[i].relationships[j].attributes.name;

                                    break;
                                case "cover_art":
                                    getMangaPoster = manga.data[i].relationships[j].attributes.fileName;
                                    break;
                                default:
                                    break;
                            }
                        }

                        Manga.Add(new Manga
                        {
                            id = manga.data[i].id,
                            type = manga.data[i].type,
                            author = getMangaAuthor,
                            poster = $"https://uploads.mangadex.org/covers/{manga.data[i].id}/{getMangaPoster}",
                            attributes = manga.data[i].attributes,
                            relationships = manga.data[i].relationships
                        });

                        //System.Diagnostics.Debug.WriteLine(manga.data[i].attributes.title.en);
                        //System.Diagnostics.Debug.WriteLine(manga.data[i].relationships[2].id);
                        //System.Diagnostics.Debug.WriteLine(manga.data[i].relationships[2].type);
                        //System.Diagnostics.Debug.WriteLine(manga.data[i].relationships[2].attributes.fileName);
                    }
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    public string SplitString(string value)
    {
        return "";
    }
}

