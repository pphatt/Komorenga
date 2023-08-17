using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Komorenga.Views;
using Newtonsoft.Json;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.ViewModels;

internal class ReadingPageViewModels
{
    public ObservableCollection<Manga> Manga { get; } = new();

    public ReadingPageViewModels()
    {
        WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
        {
            _ = FetchData($"https://api.mangadex.org/manga/{m}?includes[]=artist&includes[]=author&includes[]=cover_art", Manga);
        });
    }

    private async Task FetchData(string url, ObservableCollection<Manga> mangas)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    SingleMangaJSON manga = JsonConvert.DeserializeObject<SingleMangaJSON>(responseData);

                    List<string> relationship = GetCurrentMangaRelationship(manga.data);

                    System.Diagnostics.Debug.WriteLine(manga.data.attributes.altTitles);

                    mangas.Add(new Manga
                    {
                        id = manga.data.id,
                        type = manga.data.type,
                        author = relationship[0],
                        poster = $"https://uploads.mangadex.org/covers/{manga.data.id}/{relationship[2]}",
                        attributes = manga.data.attributes,
                        relationships = manga.data.relationships
                    });
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

    private List<string> GetCurrentMangaRelationship(Manga manga)
    {
        List<string> relationships = new List<string>();

        var getMangaAuthor = "";
        var getMangaArtist = "";
        var getMangaPoster = "";

        for (var j = 0; j < manga.relationships.Count; j++)
        {
            switch (manga.relationships[j].type)
            {
                case "author":
                    if (getMangaAuthor.Length > 0)
                    {
                        getMangaAuthor += ", ";
                    }

                    getMangaAuthor += manga.relationships[j].attributes.name;

                    break;
                case "artist":
                    if (getMangaArtist.Length > 0)
                    {
                        getMangaArtist += ", ";
                    }

                    getMangaArtist += manga.relationships[j].attributes.name;
                    break;
                case "cover_art":
                    getMangaPoster = manga.relationships[j].attributes.fileName;
                    break;
                default:
                    break;
            }
        }

        relationships.Add(getMangaAuthor);
        relationships.Add(getMangaArtist);
        relationships.Add(getMangaPoster);

        return relationships;
    }
}
