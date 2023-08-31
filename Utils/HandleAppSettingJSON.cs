using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komorenga.Models;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Windows.Storage;

namespace Komorenga.Utils;

class HandleAppSettingJSON
{
    private static HandleAppSettingJSON instance;

    private static readonly string ConfigFileName = "mangaMarked.json";

    private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

    public static HandleAppSettingJSON Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HandleAppSettingJSON();
            }

            return instance;
        }
    }

    public async Task SaveMangaMarkedAsync<T>(T manga)
    {
        StorageFile jsonFile = await localFolder.CreateFileAsync(ConfigFileName, CreationCollisionOption.ReplaceExisting);

        string json = JsonConvert.SerializeObject(manga);
        await FileIO.WriteTextAsync(jsonFile, json);
    }

    public async Task<List<LibraryMangaBookMarked>> LoadMangaMarkedAsync()
    {
        try
        {
            StorageFile jsonFile = await localFolder.GetFileAsync(ConfigFileName);

            string json = await FileIO.ReadTextAsync(jsonFile);
            return JsonConvert.DeserializeObject<List<LibraryMangaBookMarked>>(json);
        } catch (FileNotFoundException)
        {
            StorageFile jsonFile = await localFolder.CreateFileAsync(ConfigFileName);

            await FileIO.WriteTextAsync(jsonFile, "[]");

            return new List<LibraryMangaBookMarked>();
        }
    }
}
