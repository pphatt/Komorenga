using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Komorenga.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using static Komorenga.Utils.HandleAppSettingJSON;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Komorenga.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Reading : Page
    {
        public Reading()
        {
            this.InitializeComponent();
        }

        private void ReadChap(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is MangaChapterVolume chapter)
            {
                MangaChapterVolumeAttributes attributes = (MangaChapterVolumeAttributes)button.Tag;

                ObservableCollection<Manga> manga = ViewModel.Manga;

                var m_window = new Window();
                m_window.Content = new ReadingMangaPage(manga, chapter, m_window);
                m_window.Title = GetCurrentChapterTitle(attributes);
                m_window.Activate();

                WeakReferenceMessenger.Default.Send(chapter);
            }
        }

        private string GetCurrentChapterTitle(MangaChapterVolumeAttributes attributes)
        {
            string title = "";

            if (attributes.volume != null)
            {
                title += $"Vol. {attributes.volume} ";
            }

            if (attributes.chapter != null)
            {
                title += $"Ch. {attributes.chapter} ";
            }

            if (attributes.title != null)
            {
                title += $"- {attributes.title}";
            }

            return title;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LibraryMangaBookMarked bookMarked = new LibraryMangaBookMarked { 
                id = ViewModel.Manga[0].id, 
                title = GetTitleByLanguage(ViewModel.Manga[0].attributes.title), 
                posterUrl = ViewModel.Manga[0].poster, 
                author = ViewModel.Manga[0].author 
            };

            List<LibraryMangaBookMarked> manga = await Instance.LoadMangaMarkedAsync();

            manga.Add(bookMarked);

            await Instance.SaveMangaMarkedAsync(manga);
        }

        private string GetTitleByLanguage(Language currentAttributeTitle)
        {
            if (currentAttributeTitle.en != null)
            {
                return currentAttributeTitle.en;
            }

            if (currentAttributeTitle.jaRo != null)
            {
                return currentAttributeTitle.jaRo;
            }

            return "Something went wrong... Try again";
        }
    }
}
