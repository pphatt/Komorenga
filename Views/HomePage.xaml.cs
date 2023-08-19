using System.Net.Http;
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

    private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Grid grid && grid.DataContext is Manga manga)
        {
            Shell.CurrentShell.SetContentFrame(typeof(Reading));

            WeakReferenceMessenger.Default.Send(manga.id);
        }
    }
}