using System.Net.Http;
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
    //ScrollViewer scrollViewer = ?.FindFirstChild<ScrollViewer>();
    private readonly HttpClient _httpClient;

    public HomePage()
    {
        this.InitializeComponent();

        var url = "https://api.mangadex.org/manga?" +
            "includes[]=cover_art&" +
            "includes[]=artist&" +
            "includes[]=author&" +
            "order[followedCount]=desc&" +
            "contentRating[]=safe&" +
            "contentRating[]=suggestive&" +
            "hasAvailableChapters=true&" +
            "createdAtSince=2023-07-10T03%3A06%3A02";

        // https://api.mangadex.org/manga?
        // includes[]=cover_art&
        // includes[]=artist&
        // includes[]=author&
        // order[followedCount]=desc&
        // contentRating[]=safe&
        // contentRating[]=suggestive&
        // hasAvailableChapters=true&
        // createdAtSince=2023-07-14T09%3A25%3A06

        // https://api.mangadex.org/manga?
        // includes[]=cover_art&
        // includes[]=artist&
        // includes[]=author&
        // order[followedCount]=desc&
        // contentRating[]=safe&
        // contentRating[]=suggestive&
        // hasAvailableChapters=true&
        // createdAtSince=2023-07-14T12%3A14%3A50

        // 2023-07-14T12%3A18%3A01
        // 2023-07-14T12%3A19%3A04



        // bf0e6911-d03c-4b3c-8ee1-32e6220ba4a6
        // 6d48d4cb-41e6-452b-a0be-c159d10ac674.png
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

    private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
    {

    }

    private void Grid_PointerPressed_1(object sender, PointerRoutedEventArgs e)
    {

    }
}