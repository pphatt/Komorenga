using System;
using System.Collections.Generic;
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
                var m_window = new Window();
                m_window.Content = new ReadingMangaPage();
                m_window.Title = GetCurrentReadingMangaTitle(button);
                m_window.Activate();

                WeakReferenceMessenger.Default.Send(chapter);
            }
        }

        private string GetCurrentReadingMangaTitle(Button button)
        {
            string title = "";

            MangaChapterVolumeAttributes ButtonTag = (MangaChapterVolumeAttributes)button.Tag;

            if (ButtonTag.volume != null)
            {
                title += $"Vol. {ButtonTag.volume} ";
            }

            if (ButtonTag.chapter != null)
            {
                title += $"Ch. {ButtonTag.chapter} ";
            }

            if (ButtonTag.title != null)
            {
                title += $"- {ButtonTag.title}";
            }

            return title;
        }
    }
}
