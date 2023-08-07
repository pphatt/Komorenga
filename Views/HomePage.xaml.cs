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
    }
}
