using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Newtonsoft.Json;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.ViewModels;

class SearchPageViewModels : INotifyPropertyChanged
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

    private ObservableCollection<Manga> AdvanceSearchMangaCollection;

    public ObservableCollection<Manga> AdvanceSearchManga
    {
        get => AdvanceSearchMangaCollection;
        set
        {
            AdvanceSearchMangaCollection = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdvanceSearchManga)));
        }
    }

    private CancellationTokenSource cancellationTokenSource = new();

    public SearchPageViewModels()
    {
        AdvanceSearchMangaCollection = new();

        LoadFetchData();
    }

    public async Task AdvanceSearchMangaAsync(string title, string sort)
    {
        try
        {
            IsLoading = true;
            AdvanceSearchMangaCollection.Clear();

            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            await Task.Delay(1500, cancellationTokenSource.Token);

            cancellationTokenSource.Token.ThrowIfCancellationRequested();

            string QueryURL = title == "" ?
               $"https://api.mangadex.org/manga?limit=100&offset=0&includes[]=cover_art&includes[]=artist&includes[]=author&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&order{sort}" :
               $"https://api.mangadex.org/manga?limit=100&offset=0&includes[]=cover_art&includes[]=artist&includes[]=author&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&title={title}&order{sort}";

            System.Diagnostics.Debug.WriteLine("Title: " + title);

            List<Manga> SearchManga = await FetchData(QueryURL);

            IsLoading = false;

            if (SearchManga == null)
            {
                return;
            }

            foreach (var manga in SearchManga)
            {
                this.AdvanceSearchManga.Add(manga);
            }
        } catch (TaskCanceledException ex)
        {
            System.Diagnostics.Debug.WriteLine("Task Canceled: " + ex.Message);
        }
    }

    public async void LoadFetchData()
    {
        IsLoading = true;

        Task<List<Manga>> SearchMangaAPICall = FetchData("https://api.mangadex.org/manga?limit=100&offset=0&includes[]=cover_art&includes[]=artist&includes[]=author&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&order[latestUploadedChapter]=desc");

        List<Manga> SearchManga = await SearchMangaAPICall;

        for (int i = 0; i < SearchManga.Count; i++)
        {
            this.AdvanceSearchManga.Add(SearchManga[i]);
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
