using System;
using System.Linq;
using Komorenga.Utils;
using Komorenga.Views;
using Microsoft.UI;           // Needed for WindowId.
using Microsoft.UI.Windowing; // Needed for AppWindow.
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;
using WinRT.Interop;          // Needed for XAML/HWND interop.

namespace Komorenga;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Shell : Window
{
    private AppWindow m_AppWindow;

    public static Shell CurrentShell;

    public Shell()
    {
        CurrentShell = this;

        this.InitializeComponent();

        Title = "Komorenga";

        // Check to see if customization is supported.
        // The method returns true on Windows 10 since Windows App SDK 1.2, and on all versions of
        // Windows App SDK on Windows 11.
        m_AppWindow = GetAppWindowForCurrentWindow();
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = m_AppWindow.TitleBar;
            // Hide default title bar.
            titleBar.ExtendsContentIntoTitleBar = true;

            titleBar.ButtonBackgroundColor = new Color() { R = 31, G = 31, B = 31 };
            //titleBar.ButtonHoverBackgroundColor = new Color() { R = 77, G = 77, B = 77 };
        }
        else
        {
            // In the case that title bar customization is not supported, hide the custom title bar
            // element.
            AppTitleBar.Visibility = Visibility.Collapsed;
        }

        NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems.OfType<NavigationViewItem>().First();
        ContentFrame.Navigate(typeof(HomePage), null, new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
    }

    private AppWindow GetAppWindowForCurrentWindow()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    public string GetAppTitleFromSystem()
    {
        return Windows.ApplicationModel.Package.Current.DisplayName;
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private void NavigationViewControl_ItemInvoked(NavigationView sender,
                  NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked == true)
        {
            ContentFrame.Navigate(typeof(Setting), null, args.RecommendedNavigationTransitionInfo);
        }
        else if (args.InvokedItemContainer != null && args.InvokedItemContainer.Tag != null)
        {
            Type newPage = Type.GetType(args.InvokedItemContainer.Tag.ToString());
            ContentFrame.Navigate(newPage, null, args.RecommendedNavigationTransitionInfo);
        }
    }

    private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (ContentFrame.CanGoBack) ContentFrame.GoBack();
    }

    public void SetContentFrame(Type type, object value = null)
    {
        ContentFrame.Navigate(type, value);
    }
}
