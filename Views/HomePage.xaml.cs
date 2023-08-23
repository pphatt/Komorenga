using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Komorenga.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : Page
{
    public HomePage()
    {
        this.InitializeComponent();
    }

    private void PopularNewTitleNextScroll(object sender, RoutedEventArgs e)
    {
        PopularNewTitleScrollViewer.ChangeView(PopularNewTitleScrollViewer.HorizontalOffset + 200, 0, 1.0f);
    }

    private void PopularNewTitleBackwardScroll(object sender, RoutedEventArgs e)
    {
        PopularNewTitleScrollViewer.ChangeView(PopularNewTitleScrollViewer.HorizontalOffset - 200, 0, 1.0f);
    }

    private void SeasonalNextScroll(object render, RoutedEventArgs e)
    {
        SeasonalScrollViewer.ChangeView(SeasonalScrollViewer.HorizontalOffset + 200, 0, 1.0f);
    }

    private void SeasonalBackwardScroll(object render, RoutedEventArgs e)
    {
        SeasonalScrollViewer.ChangeView(SeasonalScrollViewer.HorizontalOffset - 200, 0, 1.0f);
    }

    private void MostFollowsNextScroll(object render, RoutedEventArgs e)
    {
        MostFollowsScrollViewer.ChangeView(MostFollowsScrollViewer.HorizontalOffset + 200, 0, 1.0f);
    }

    private void MostFollowsBackwardScroll(object render, RoutedEventArgs e)
    {
        MostFollowsScrollViewer.ChangeView(MostFollowsScrollViewer.HorizontalOffset - 200, 0, 1.0f);
    }

    private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Grid grid && grid.DataContext is Manga manga)
        {
            Shell.CurrentShell.SetContentFrame(typeof(Reading));

            WeakReferenceMessenger.Default.Send(manga.id);
        }
    }

    private void AutoSuggest_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {

    }

    private async void AutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        var input = sender.Text.Trim();

        List<Manga> SearchManga = await ViewModel.SearchMangaAsync(input);

        if (SearchManga.Count == 0)
        {
            sender.ItemsSource = new List<string> { "No results found" };
            return;
        }

        sender.ItemsSource = ParserMangaTitle(SearchManga);
    }

    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        foreach (Manga manga in ViewModel.SearchManga)
        {
            if (manga.attributes.title.en == args.SelectedItem.ToString() || 
                manga.attributes.title.jaRo == args.SelectedItem.ToString())
            {
                Shell.CurrentShell.SetContentFrame(typeof(Reading));

                WeakReferenceMessenger.Default.Send(manga.id);
            }
        }
    }

    private void AutoSuggest_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        AutoSuggestBox.Focus(FocusState.Programmatic);
    }

    private List<string> ParserMangaTitle(List<Manga> manga)
    {
        List<string> titles = new();

        foreach(Manga m in manga)
        {
            if (m.attributes.title.en != null)
            {
                titles.Add(m.attributes.title.en);
                continue;
            }

            if (m.attributes.title.jaRo != null)
            {
                titles.Add(m.attributes.title.jaRo);
            }
        }

        return titles;
    }
}