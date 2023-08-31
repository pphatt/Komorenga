using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Komorenga.Models;
using Komorenga.Utils;
using Newtonsoft.Json;
using Windows.Storage;
using static Komorenga.Models.MangaJSONModels;

namespace Komorenga.ViewModels;

class LibraryViewModels : INotifyPropertyChanged
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

    private ObservableCollection<LibraryMangaBookMarked> BookMarkedCollection;

    public ObservableCollection<LibraryMangaBookMarked> BookMarked
    {
        get => BookMarkedCollection;
        set
        {
            BookMarkedCollection = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BookMarked)));
        }
    }

    public LibraryViewModels()
    {
        BookMarkedCollection = new ObservableCollection<LibraryMangaBookMarked>();

        LoadLibraryBookMarked();
    }

    public async void LoadLibraryBookMarked()
    {
        IsLoading = true;

        System.Diagnostics.Debug.WriteLine("Yhea");

        List<LibraryMangaBookMarked> LoadBookmarkedCollection = await HandleAppSettingJSON.Instance.LoadMangaMarkedAsync();

        foreach (var manga in LoadBookmarkedCollection)
        {
            BookMarked.Add(manga);
        }

        IsLoading = false;

        //StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        //StorageFile jsonFile = await localFolder.GetFileAsync("mangaMarked.json");

        //string json = await FileIO.ReadTextAsync(jsonFile);

        //System.Diagnostics.Debug.WriteLine(json);
        //List<LibraryMangaBookMarked> a = JsonConvert.DeserializeObject<List<LibraryMangaBookMarked>>(json);
        //System.Diagnostics.Debug.WriteLine("A: " + a.Count);
    }
}
