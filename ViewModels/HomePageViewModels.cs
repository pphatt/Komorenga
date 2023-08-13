using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        FetchData("https://api.mangadex.org/manga?includes[]=cover_art&includes[]=artist&includes[]=author&order[followedCount]=desc&contentRating[]=safe&contentRating[]=suggestive&hasAvailableChapters=true&createdAtSince=2023-07-10T03%3A06%3A02");
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

                    System.Diagnostics.Debug.WriteLine(responseData);

                    MangaJSON manga = JsonConvert.DeserializeObject<MangaJSON>(responseData);

                    System.Diagnostics.Debug.WriteLine(manga.result);

                    for (int i = 0; i < manga.data.Count; i++)
                    {
                        Manga.Add(new Manga
                        {
                            id = manga.data[i].id,
                            type = manga.data[i].type,
                            attributes = manga.data[i].attributes,
                            poster = $"https://uploads.mangadex.org/covers/{manga.data[i].id}/{manga.data[i].relationships[2].attributes.fileName}",
                            relationships = manga.data[i].relationships
                        });

                        System.Diagnostics.Debug.WriteLine(manga.data[i].attributes.title.en);

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

