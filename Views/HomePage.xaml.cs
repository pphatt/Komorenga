using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using Komorenga.Models;
using static Komorenga.Models.MangaJSONModels;
using System.Collections.ObjectModel;
using Komorenga.ViewModels;

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

        if (scrollViewer != null)
        {
            // Attach the event handler to the ViewChanged event
            scrollViewer.ViewChanged += OnScrollViewChanged;
        }

        // The event handler for the ViewChanged event
        void OnScrollViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            // Get the vertical and horizontal scroll offsets
            double verticalOffset = scrollViewer.VerticalOffset;
            double horizontalOffset = scrollViewer.HorizontalOffset;

            // Use the offsets as needed
            // For example, you can print them out
            System.Diagnostics.Debug.WriteLine("Vertical Offset: " + verticalOffset);
            System.Diagnostics.Debug.WriteLine("Horizontal Offset: " + horizontalOffset);
        }

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
        
        
        
        // bf0e6911-d03c-4b3c-8ee1-32e6220ba4a6
        // 6d48d4cb-41e6-452b-a0be-c159d10ac674.png
    }

    private void TestNext(object sender, RoutedEventArgs e)
    {
        bool check = scrollViewer.ChangeView(scrollViewer.HorizontalOffset + 200, 0, 1.0f);

        System.Diagnostics.Debug.WriteLine(check);
    }

    private void TestBackward(object sender, RoutedEventArgs e)
    {
        bool check = scrollViewer.ChangeView(scrollViewer.HorizontalOffset - 200, 0, 1.0f);

        System.Diagnostics.Debug.WriteLine(check);
    }
}