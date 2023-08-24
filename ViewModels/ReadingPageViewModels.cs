using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Newtonsoft.Json;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.ViewModels;

internal class ReadingPageViewModels : INotifyPropertyChanged
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

    private ObservableCollection<Manga> MangaData;

    public ObservableCollection<Manga> Manga
    {
        get => MangaData;
        set
        {
            MangaData = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Manga)));
        }
    }

    public ReadingPageViewModels()
    {
        MangaData = new ObservableCollection<Manga>();

        LoadFetchData();
    }

    private void LoadFetchData()
    {
        WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
        {
            IsLoading = true;

            Task<List<Manga>> MangaDetailsAPIClient = FetchData($"https://api.mangadex.org/manga/{m}?includes[]=artist&includes[]=author&includes[]=cover_art");

            List<Manga> MangaDetails = await MangaDetailsAPIClient;

            Manga.Add(MangaDetails[0]);

            IsLoading = false;

            WeakReferenceMessenger.Default.Unregister<string>(this);
        });
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

                    SingleMangaJSON manga = JsonConvert.DeserializeObject<SingleMangaJSON>(responseData);

                    List<string> relationship = GetCurrentMangaRelationship(manga.data);

                    List<Manga> Manga = new List<Manga>();

                    List<MangaChapterVolume> Chapter = new();

                    for (var i = 0; i < 2; i++)
                    {
                        HttpResponseMessage chapterResponse = await httpClient.GetAsync($"https://api.mangadex.org/manga/{manga.data.id}/feed?limit=500&translatedLanguage[]=en&includes[]=scanlation_group&includes[]=user&includeExternalUrl=0&order[volume]=desc&order[chapter]=desc&offset={i * 500}&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&contentRating[]=pornographic");
                        
                        if (chapterResponse.IsSuccessStatusCode)
                        {
                            string chapterResponseData = await chapterResponse.Content.ReadAsStringAsync();

                            System.Diagnostics.Debug.WriteLine(chapterResponseData);

                            MangaChapterResponse mangaChapter = JsonConvert.DeserializeObject<MangaChapterResponse>(chapterResponseData);

                            Chapter.AddRange(mangaChapter.data);
                        }
                    }

                    Manga.Add(new Manga
                    {
                        id = manga.data.id,
                        type = manga.data.type,
                        author = relationship[0],
                        poster = $"https://uploads.mangadex.org/covers/{manga.data.id}/{relationship[1]}",
                        attributes = manga.data.attributes,
                        relationships = manga.data.relationships,
                        chapter = Chapter
                    });

                    return Manga;
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }

    private List<string> GetCurrentMangaRelationship(Manga manga)
    {
        List<string> getMangaAuthorAndArtist = new();

        string getMangaPoster = "";

        for (var j = 0; j < manga.relationships.Count; j++)
        {
            switch (manga.relationships[j].type)
            {
                case "author":
                case "artist":
                    if (!getMangaAuthorAndArtist.Contains(manga.relationships[j].attributes.name))
                    {
                        getMangaAuthorAndArtist.Add(manga.relationships[j].attributes.name);
                    }

                    break;
                case "cover_art":
                    getMangaPoster += manga.relationships[j].attributes.fileName;
                    break;
                default:
                    break;
            }
        }

        return new List<string> { String.Join(", ", getMangaAuthorAndArtist.ToArray()), getMangaPoster };
    }
}
