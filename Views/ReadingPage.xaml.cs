using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using static Komorenga.Models.MangaJSONModels;

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
    }
}
