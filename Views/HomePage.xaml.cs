using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Komorenga.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : Page
{
    //ScrollViewer scrollViewer = ?.FindFirstChild<ScrollViewer>();

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

        //if (scrollViewer != null)
        //{
        //    // Set the initial scroll position (for example, scroll to the top left corner)
        //    scrollViewer.ChangeView(horizontalOffset: 0, verticalOffset: 0, zoomFactor: null);
        //}

        // You can then change the scroll position programmatically by calling the ChangeView method with the desired offsets
        //void SetScrollPosition(double horizontalOffset, double verticalOffset)
        //{
        //    // Change the scroll position to the desired offsets
        //    scrollViewer?.ChangeView(horizontalOffset, verticalOffset, zoomFactor: null);
        //}

        //SetScrollPosition(100, 0);
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
