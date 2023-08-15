using System.Net.Http;
using Komorenga.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using CommunityToolkit.Mvvm.Messaging;

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
        if (sender is Grid grid && grid.DataContext is Manga manga)
        {
            //System.Diagnostics.Debug.WriteLine(manga.id);
            //System.Diagnostics.Debug.WriteLine(manga.attributes.title.en);
            //System.Diagnostics.Debug.WriteLine("");
            //App.CurrentShell.SetContentFrame(typeof(Reading));

            Shell.CurrentShell.SetContentFrame(typeof(Reading));

            WeakReferenceMessenger.Default.Send(manga.id);
        }
    }
}