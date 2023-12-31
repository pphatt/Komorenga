﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Newtonsoft.Json;

namespace Komorenga.ViewModels;

class SettingViewModels
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

    public SettingViewModels()
    {
        ChapterData = new ObservableCollection<MangaChapterImageUrl>();

        _ = LoadFetchData();
    }

    private async Task LoadFetchData()
    {
        WeakReferenceMessenger.Default.Register<MangaChapterVolume>(this, async (r, m) =>
        {
            IsLoading = true;

            Task<List<MangaChapterImageUrl>> MangaChapterImageAPIClient = FetchData($"https://api.mangadex.org/at-home/server/{m.id}");

            List<MangaChapterImageUrl> MangaChapterImage = await MangaChapterImageAPIClient;

            for (int i = 0; i < MangaChapterImage.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(MangaChapterImage[i].url);
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

                    for (int i = 0; i < chapterHashDataUrl.Count; i++)
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
