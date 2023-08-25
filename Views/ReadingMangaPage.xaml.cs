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
using System.Threading.Tasks;

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
        this.shell = shell;

        Get("");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Get("previous");

        ViewModel.GetChapter(CurrentChapter.id);

        shell.Title = GetCurrentChapterTitle();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        Get("next");

        ViewModel.GetChapter(CurrentChapter.id);

        shell.Title = GetCurrentChapterTitle();
    }

    private void PopularNewTitleScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(PopularNewTitleScrollViewer.VerticalOffset);
    }

    private async void ItemControl_Loaded(object sender, RoutedEventArgs e)
    {
        await Task.Delay(1000);

        List<double> gridHeights = new();

        foreach (var item in ItemControl.Items)
        {
            var container = ItemControl.ContainerFromItem(item) as ContentPresenter;

            if (container != null)
            {
                var grid = FindVisualChild<Grid>(container);

                if (grid != null)
                {
                    double gridHeight = grid.ActualHeight;
                    gridHeights.Add(gridHeight);
                }
            }
        }

        foreach (double height in gridHeights)
        {
            System.Diagnostics.Debug.WriteLine($"Grid Height: {height}");
        }
    }

    private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T result)
            {
                return result;
            }
            else
            {
                T descendant = FindVisualChild<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }
        }
        return null;
    }

    private void Get(string direct)
    {
        List<MangaChapterVolume> chapter = Manga[0].chapter;

        for (var i = 0; i < chapter.Count; i++)
        {
            if (chapter[i].id == CurrentChapter.id)
            {
                switch (direct)
                {
                    case "next":
                        if (i > 0)
                        {
                            CurrentChapter = chapter[--i];

                            Next_Chapter_Button.IsEnabled = true;
                            Previous_Chapter_Button.IsEnabled = true;
                        }

                        break;
                    case "previous":
                        if (i < chapter.Count)
                        {
                            CurrentChapter = chapter[++i];

                            Next_Chapter_Button.IsEnabled = true;
                            Previous_Chapter_Button.IsEnabled = true;
                        }

                        break;
                    default:
                        break;
                }

                if (i == 0)
                {
                    Next_Chapter_Button.IsEnabled = false;
                }

                if (i == chapter.Count - 1)
                {
                    Previous_Chapter_Button.IsEnabled = false;
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
            title += $"Vol. {CurrentChapter.attributes.volume} ";
        }

        if (CurrentChapter.attributes.chapter != null)
        {
            title += $"Ch. {CurrentChapter.attributes.chapter} ";
        }

        if (CurrentChapter.attributes.title != null)
        {
            title += $"- {CurrentChapter.attributes.title}";
        }

        return title;
    }
}
