using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Komorenga.Models;
using Newtonsoft.Json;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.ViewModels;

internal class HomePageViewModels : INotifyPropertyChanged
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

    private ObservableCollection<Manga> PopularNewTitleMangaCollection;
    private ObservableCollection<Manga> SeasonalMangaCollection;

    public ObservableCollection<Manga> PopularNewTitleManga
    {
        get => PopularNewTitleMangaCollection;
        set
        {
            PopularNewTitleMangaCollection = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PopularNewTitleManga)));
        }
    }

    public ObservableCollection<Manga> SeasonalManga
    {
        get => SeasonalMangaCollection;
        set
        {
            SeasonalMangaCollection = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeasonalManga)));
        }
    }

    public HomePageViewModels()
    {
        PopularNewTitleMangaCollection = new ObservableCollection<Manga>();
        SeasonalMangaCollection = new ObservableCollection<Manga>();

        _ = LoadFetchData();
    }

    private async Task LoadFetchData()
    {
        IsLoading = true;

        DateTime utcTime = DateTime.UtcNow.AddMonths(-1).AddDays(1);
        var encodedUtcTime = HttpUtility.UrlEncode(utcTime.ToString("yyyy-MM-ddTHH:mm:ss")).ToUpper();

        //var url = "https://api.mangadex.org/manga?" +
        //    "includes[]=cover_art&" +
        //    "includes[]=artist&" +
        //    "includes[]=author&" +
        //    "order[followedCount]=desc&" +
        //    "contentRating[]=safe&" +
        //    "contentRating[]=suggestive&" +
        //    "hasAvailableChapters=true&" +
        //    "createdAtSince=2023-07-10T03%3A06%3A02";

        // https://api.mangadex.org/manga?
        // includes[]=cover_art&
        // includes[]=artist&
        // includes[]=author&
        // order[followedCount]=desc&
        // contentRating[]=safe&
        // contentRating[]=suggestive&
        // hasAvailableChapters=true&
        // createdAtSince=2023-07-14T09%3A25%3A06

        // https://api.mangadex.org/manga?
        // includes[]=cover_art&
        // includes[]=artist&
        // includes[]=author&
        // order[followedCount]=desc&
        // contentRating[]=safe&
        // contentRating[]=suggestive&
        // hasAvailableChapters=true&
        // createdAtSince=2023-07-14T12%3A14%3A50

        // 2023-07-14T12%3A18%3A01
        // 2023-07-14T12%3A19%3A04

        // bf0e6911-d03c-4b3c-8ee1-32e6220ba4a6
        // 6d48d4cb-41e6-452b-a0be-c159d10ac674.png

        Task<List<Manga>> PopularNewTitleMangaAPICall = FetchData($"https://api.mangadex.org/manga?includes[]=cover_art&includes[]=artist&includes[]=author&order[followedCount]=desc&contentRating[]=safe&contentRating[]=suggestive&hasAvailableChapters=true&createdAtSince={encodedUtcTime}");

        Task<List<Manga>> SeasonalMangaAPICall = FetchData("https://api.mangadex.org/manga?ids[]=dc0359ea-c202-4906-af43-9cf204b2f3da&ids[]=a053778e-4a79-4954-b6cc-3c5a43af8310&ids[]=f1caebaa-d5b6-4eb4-8dad-12ae0eb232e9&ids[]=62f7fdd6-1197-4ca2-b4cc-adb648882a74&ids[]=ad4bc67f-fcc5-476e-99e7-27cea05ad421&ids[]=57bda7ca-b19e-4912-9ecd-e4082eca1665&ids[]=051a49d7-dbd8-45eb-bf2a-4c04ab3e2e88&ids[]=a6d25f47-17e2-49d9-b690-296cb4cac8ed&ids[]=09d29299-40cd-47b9-ae1a-fa8c70e520ce&ids[]=3c60b496-2168-4bde-be85-2ce75ecb441e&ids[]=94a48b9c-2a5b-4397-95f1-15df8b017488&ids[]=6f9ef57a-0502-4f44-b341-86423c90cb4a&ids[]=9ea92521-d38b-4f66-a1bd-7e653e93ef98&ids[]=2678cc3f-eee1-4659-9202-28c28bc2e141&ids[]=cef6253d-6a55-4438-921c-46968b426f2f&ids[]=0eeea586-b7db-4bbf-893e-c84f52050d2a&ids[]=8fc6f422-d693-4dc1-ab17-545f500e010e&ids[]=99e16a63-6ec4-4760-85ed-e2f42f1b8d06&ids[]=978b81a4-7abf-42c6-a3a9-a8d7392ee2de&ids[]=8aa1fb77-c86e-41db-adfb-0737c3438113&ids[]=2c58fb86-7fb9-4db6-9c5f-9696934ea2cb&ids[]=169f959e-c8d1-4341-86b3-ac0c263f709e&ids[]=45a4f34b-ed6e-49d2-9ba2-d60b471dcefe&ids[]=701e67c9-45b2-459b-a517-a0cb9865def8&ids[]=bc94b9a7-e17e-4a00-be92-882ba520cb85&ids[]=167dd939-ec5b-4e57-b8c4-1351dd065d82&ids[]=d64b0768-bebe-433c-b923-99870503c0a3&ids[]=bb790a3b-9232-4552-97c4-50040f62c960&ids[]=9484b1fd-0271-4c9b-b096-7e313823058e&ids[]=b9b2fbc4-e351-406c-a468-799be14033df&ids[]=efd02206-1cd8-4823-b497-02bb8b8d09ca&ids[]=188e4f34-a80c-4a91-b54e-69572e8ed4d5&ids[]=6d6d9fda-5cd3-40ac-948b-776b1a1a0eb1&ids[]=14a75df9-07d2-4181-acc0-5693be1aeb3e&ids[]=d52a2ce5-5ea1-4bef-9a0e-58519caaecd8&ids[]=48496326-c504-459a-9f95-1c97d25edb68&ids[]=44f84cc1-819c-47a6-a59f-43d016f72b43&ids[]=c52b2ce3-7f95-469c-96b0-479524fb7a1a&ids[]=99c55b76-dee9-4758-a0c9-ed1a70d4edca&ids[]=0dac60e0-1358-44dd-88cf-927a5fcff348&ids[]=7e10d5f8-9888-44ac-9921-e7055bbcb12e&ids[]=0e017a08-835a-4cbe-ba63-576d5010a5a0&ids[]=dc5b9e42-1840-4307-998d-e0dfcfe2b9e6&ids[]=e39944f5-15bf-4464-9556-a4e9b3945571&ids[]=30f3ac69-21b6-45ad-a110-d011b7aaadaa&ids[]=7f952b22-812d-4252-88e1-a99818f13acd&ids[]=32fdfe9b-6e11-4a13-9e36-dcd8ea77b4e4&ids[]=8d5bac78-018f-4f23-8a8d-0f25c9d6058f&ids[]=b3949b7e-a3c1-4532-b571-279b0f44a01d&ids[]=a2febd3e-6252-46eb-bd63-01d51deaaec5&ids[]=4beb73b3-13c8-4b2a-8bbc-8e42514b3b38&ids[]=b4441847-6e9d-45b7-ac3f-fe7832e27821&ids[]=b15632d7-88d0-4233-9815-c01e75cabda8&ids[]=3e9fefd8-d316-4e9c-9fb7-143814c04c17&ids[]=10a4985d-0713-462e-a9d6-767bf91e4fd7&ids[]=47216c61-007f-4177-b801-a1f3e5c5245e&ids[]=e78a489b-6632-4d61-b00b-5206f5b8b22b&ids[]=e7eabe96-aa17-476f-b431-2497d5e9d060&ids[]=3397fcf6-18bb-4983-8daa-2b0ccd79487e&ids[]=9f7530e3-3fc0-4d2b-a1f9-00caadbf2d05&ids[]=bd6d0982-0091-4945-ad70-c028ed3c0917&ids[]=3fba42cf-2ad6-4c30-a7ab-46cb8149208a&ids[]=38e121fe-2a2e-40d8-8a0a-e91d79a99ca9&ids[]=21a8efa7-8b64-41de-b61c-b4f94e2aed62&ids[]=8e6b0382-49d7-4131-8f4b-2e4df8a38102&ids[]=bb5d9e0b-3736-432b-b3fe-dc08920243e9&ids[]=e52d9403-3356-403b-b7bb-d7d6a420dd50&ids[]=a25e46ec-30f7-4db6-89df-cacbc1d9a900&ids[]=98052ef1-49a9-4722-a8f5-60baac437e2e&ids[]=88d8271a-ed40-4874-b23f-a1ded5fc1ebf&ids[]=0ee2f134-87c6-4715-b31e-b3b1344fa5ea&ids[]=f801e9c5-b9dc-4aee-987e-fcc16eef2f45&ids[]=e36da8b0-feca-46dd-9120-d5dc2f3feae8&ids[]=754a46fa-62fa-457a-bc3b-4f31bf1373d4&ids[]=7f30dfc3-0b80-4dcc-a3b9-0cd746fac005&ids[]=a1c7c817-4e59-43b7-9365-09675a149a6f&ids[]=239d6260-d71f-43b0-afff-074e3619e3de&limit=15&contentRating[]=safe&contentRating[]=suggestive&contentRating[]=erotica&includes[]=cover_art&includes[]=artist&includes[]=author&order[followedCount]=desc&hasAvailableChapters=true");

        List<Manga> PopularNewTitleManga = await PopularNewTitleMangaAPICall;

        List<Manga> SeasonalManga = await SeasonalMangaAPICall;

        for (int i = 0; i <  PopularNewTitleManga.Count; i++)
        {
            this.PopularNewTitleManga.Add(PopularNewTitleManga[i]);
        }

        for (int i = 0; i < SeasonalManga.Count; i++)
        {
            this.SeasonalManga.Add(SeasonalManga[i]);
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

