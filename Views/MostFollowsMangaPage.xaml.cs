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
using Windows.UI.ViewManagement;
using CommunityToolkit.Mvvm.Messaging;
using Komorenga.Models;
using Komorenga.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Komorenga.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MostFollowsMangaPage : Page
    {
        public MostFollowsMangaPage()
        {
            this.InitializeComponent();
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is Manga manga)
            {
                Shell.CurrentShell.SetContentFrame(typeof(Reading));

                WeakReferenceMessenger.Default.Send(manga.id);
            }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            //var scrollViewer = sender as ScrollViewer;
            //double desiredScrollOffset = ...; // Set your desired scroll offset here

            //if (scrollViewer.VerticalOffset >= desiredScrollOffset)
            //{
            //    // Get the ViewModel instance from DataContext and call the method
            //    if (DataContext is YourViewModel viewModel)
            //    {
            //        viewModel.FetchAdditionalData();
            //    }
            //}
        }
    }
}
