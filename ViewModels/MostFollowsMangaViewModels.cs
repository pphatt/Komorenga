using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Komorenga.Models;
using Newtonsoft.Json;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.ViewModels;

class MostFollowsMangaViewModels : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private bool isLoading;

    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
        }
    }

    private ObservableCollection<Manga> MostFollowsMangaCollection;

    public ObservableCollection<Manga> MostFollowsManga
    {
        get => MostFollowsMangaCollection;
        set
        {
            MostFollowsMangaCollection = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MostFollowsManga)));
        }
    }

    public MostFollowsMangaViewModels()
    {
        MostFollowsMangaCollection = new ObservableCollection<Manga>();

        _ = LoadFetchData();
    }

    private async Task LoadFetchData()
    {
        IsLoading = true;

        Task<List<Manga>> MostFollowsMangaAPICall = FetchData("https://api.mangadex.org/manga?limit=100&offset=0&includes[]=cover_art&includes[]=artist&includes[]=author&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&order[followedCount]=desc");

        List<Manga> MostFollowsManga = await MostFollowsMangaAPICall;

        for (int i = 0; i < MostFollowsManga.Count; i++)
        {
            this.MostFollowsManga.Add(MostFollowsManga[i]);
        }

        IsLoading = false;
    }

    private async Task<List<Manga>> FetchData(string url)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", $"Komorenga/{Environment.Version.ToString()} ({Environment.OSVersion.ToString()})");

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    System.Diagnostics.Debug.WriteLine(responseData);

                    MangaJSON manga = JsonConvert.DeserializeObject<MangaJSON>(responseData);

                    List<Manga> mangas = new List<Manga>();

                    for (int i = 0; i < manga.data.Count; i++)
                    {
                        List<string> relationship = GetCurrentMangaRelationship(manga.data[i]);

                        mangas.Add(new Manga
                        {
                            id = manga.data[i].id,
                            type = manga.data[i].type,
                            author = relationship[0],
                            poster = $"https://uploads.mangadex.org/covers/{manga.data[i].id}/{relationship[2]}",
                            attributes = manga.data[i].attributes,
                            relationships = manga.data[i].relationships
                        });
                    }

                    System.Diagnostics.Debug.WriteLine(mangas.Count);

                    return mangas;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Request failed with status code: {response.RequestMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
                return null;
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
                    getMangaPoster = manga.relationships[j].attributes.fileName + ".512.jpg";
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
