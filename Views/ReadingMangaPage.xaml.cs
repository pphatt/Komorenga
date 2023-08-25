using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Komorenga.Models;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.UI.Windowing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Komorenga.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReadingMangaPage : Page
{
    private MangaChapterVolume CurrentChapter
    {
        get; set;
    }

    private string CurrentChapterID
    {
        get; set;
    } 

    private string PreviousChapterID
    {
        get; set;
    }

    private string NextChapterID
    {
        get; set;
    }

    private ObservableCollection<Manga> Manga
    {
        get; set;
    }

    private Window shell
    {
        get; set;
    }

    public ReadingMangaPage(ObservableCollection<Manga> Manga, MangaChapterVolume chapter, Window shell)
    {
        this.InitializeComponent();
        this.Manga = Manga;
        this.CurrentChapter = chapter;
        this.CurrentChapterID = chapter.id;
        this.shell = shell;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        GetPreviousChapter();

        ViewModel.GetNextAndPreviousChapter(CurrentChapterID);

        shell.Title = GetCurrentChapterTitle();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        GetNextChapter();

        ViewModel.GetNextAndPreviousChapter(CurrentChapterID);

        shell.Title = GetCurrentChapterTitle();
    }

    private void GetPreviousChapter()
    {
        System.Diagnostics.Debug.WriteLine("Current: " + CurrentChapterID);
        System.Diagnostics.Debug.WriteLine("Next: " + NextChapterID);
        System.Diagnostics.Debug.WriteLine("Previous: " + PreviousChapterID);
        System.Diagnostics.Debug.WriteLine("Title: " + CurrentChapter.attributes.title);
        System.Diagnostics.Debug.WriteLine("");

        for (var i = 0; i < Manga[0].chapter.Count; i++)
        {
            if (Manga[0].chapter[i].id == CurrentChapterID)
            {
                if (i - 1 >= 0)
                {
                    NextChapterID = Manga[0].chapter[i - 1].id;
                }

                if (i + 1 < Manga[0].chapter.Count)
                {
                    CurrentChapterID = Manga[0].chapter[i + 1].id;

                    this.CurrentChapter = Manga[0].chapter[i + 1];
                }

                break;
            }
        }

        System.Diagnostics.Debug.WriteLine("Current: " + CurrentChapterID);
        System.Diagnostics.Debug.WriteLine("Next: " + NextChapterID);
        System.Diagnostics.Debug.WriteLine("Previous: " + PreviousChapterID);
        System.Diagnostics.Debug.WriteLine("Title: " + CurrentChapter.attributes.title);
        System.Diagnostics.Debug.WriteLine("");
    }

    private void GetNextChapter()
    {
        for (var i = 0; i < Manga[0].chapter.Count; i++)
        {
            if (Manga[0].chapter[i].id == CurrentChapterID)
            {
                if (i - 1 >= 0)
                {
                    CurrentChapterID = Manga[0].chapter[i - 1].id;

                    this.CurrentChapter = Manga[0].chapter[i - 1];
                }

                if (i + 1 < Manga[0].chapter.Count)
                {
                    this.PreviousChapterID = Manga[0].chapter[i + 1].id;
                }

                break;
            }
        }
    }

    private string GetCurrentChapterTitle()
    {
        string title = "";

        if (CurrentChapter.attributes.volume != null)
        {
            title += $"Vol. {this.CurrentChapter.attributes.volume} ";
        }

        if (CurrentChapter.attributes.chapter != null)
        {
            title += $"Ch. {this.CurrentChapter.attributes.chapter} ";
        }

        if (CurrentChapter.attributes.title != null)
        {
            title += $"- {this.CurrentChapter.attributes.title}";
        }

        System.Diagnostics.Debug.WriteLine(title);

        return title;
    }
}
