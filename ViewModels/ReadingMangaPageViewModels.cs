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

class ReadingMangaPageViewModels : INotifyPropertyChanged
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

    private ObservableCollection<MangaChapterImageUrl> ChapterData;

    public ObservableCollection<MangaChapterImageUrl> Chapter
    {
        get => ChapterData;
        set
        {
            ChapterData = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Chapter)));
        }
    }

    private CancellationTokenSource cancellationTokenSource = new();

    public ReadingMangaPageViewModels()
    {
        ChapterData = new ObservableCollection<MangaChapterImageUrl>();

        LoadFetchData();
    }

    public async void GetChapter(string id)
    {
        try
        {
            IsLoading = true;
            ChapterData.Clear();

            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            await Task.Delay(500, cancellationTokenSource.Token);

            cancellationTokenSource.Token.ThrowIfCancellationRequested();

            Task<List<MangaChapterImageUrl>> MangaChapterImageAPIClient = FetchData($"https://api.mangadex.org/at-home/server/{id}");

            List<MangaChapterImageUrl> MangaChapterImage = await MangaChapterImageAPIClient;

            for (var i = 0; i < MangaChapterImage.Count; i++)
            {
                Chapter.Add(MangaChapterImage[i]);
            }

            IsLoading = false;
        } catch (TaskCanceledException ex)
        {
            System.Diagnostics.Debug.WriteLine("Task Canceled: " + ex.Message);
        }
    }

    private void LoadFetchData()
    {
        WeakReferenceMessenger.Default.Register<MangaChapterVolume>(this, async (r, m) =>
        {
            IsLoading = true;

            Task<List<MangaChapterImageUrl>> MangaChapterImageAPIClient = FetchData($"https://api.mangadex.org/at-home/server/{m.id}");

            List<MangaChapterImageUrl> MangaChapterImage = await MangaChapterImageAPIClient;

            for (int i = 0;  i < MangaChapterImage.Count; i++)
            {
                Chapter.Add(MangaChapterImage[i]);
            }

            IsLoading = false;

            WeakReferenceMessenger.Default.Unregister<MangaChapterVolume>(this);
        });
    }

    private async Task<List<MangaChapterImageUrl>> FetchData(string url)
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

                    MangaChapterImage chapter = JsonConvert.DeserializeObject<MangaChapterImage>(responseData);

                    List<string> chapterHashDataUrl = chapter.chapter.data;

                    List<MangaChapterImageUrl> returnChapterHashDataUrl = new();

                    for (var i = 0; i < chapterHashDataUrl.Count; i++)
                    {
                        returnChapterHashDataUrl.Add(new MangaChapterImageUrl
                        {
                            url = $"https://uploads.mangadex.org/data/{chapter.chapter.hash}/{chapterHashDataUrl[i]}"
                        });
                    }

                    return returnChapterHashDataUrl;
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
}
