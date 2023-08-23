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
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Windows.ApplicationModel.Activation;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Komorenga.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string search)
            {
                AutoSuggestBox.Text = search;
            }
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is Manga manga)
            {
                Shell.CurrentShell.SetContentFrame(typeof(Reading));

                WeakReferenceMessenger.Default.Send(manga.id);
            }
        }

        private void AutoSuggest_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            AutoSuggestBox.Focus(FocusState.Programmatic);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            SearchAsyncByTitle(sender.Text.Trim());
        }

        private void SearchAsyncByTitle(string search)
        {
            if (SortTypeTextBlock.Text != "Best Match")
            {
                SortTypeTextBlock.Text = "Best Match";
            }

            ViewModel.AdvanceSearchMangaAsync(search, "[relevance]=desc");
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item)
            {
                string selectedItem = item.Text;

                SortTypeTextBlock.Text = selectedItem;

                switch (selectedItem)
                {
                    case "Best Match":
                        selectedItem = "[relevance]=desc";
                        break;
                    case "Latest Upload":
                        selectedItem = "[latestUploadedChapter]=desc";
                        break;
                    case "Oldest Upload":
                        selectedItem = "[latestUploadedChapter]=asc";
                        break;
                    case "Title Ascending":
                        selectedItem = "[title]=asc";
                        break;
                    case "Title Descending":
                        selectedItem = "[title]=desc";
                        break;
                    case "Highest Rating":
                        selectedItem = "[rating]=desc";
                        break;
                    case "Lowest Rating":
                        selectedItem = "[rating]=asc";
                        break;
                    case "Most Follows":
                        selectedItem = "[followedCount]=desc";
                        break;
                    case "Fewest Follows":
                        selectedItem = "[followedCount]=asc";
                        break;
                    case "Recently Added":
                        selectedItem = "[createdAt]=desc";
                        break;
                    case "Oldest Added":
                        selectedItem = "[createdAt]=asc";
                        break;
                    case "Year Ascending":
                        selectedItem = "[year]=asc";
                        break;
                    default:
                        selectedItem = "[year]=desc";
                        break;
                }

                ViewModel.AdvanceSearchMangaAsync(AutoSuggestBox.Text.Trim(), selectedItem);
            }
        }
    }
}
