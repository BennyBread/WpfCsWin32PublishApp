using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace WpfCsWin32PublishApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openWindows = Native.GetOpenWindows();

            foreach (var window in openWindows)
            {
                MyListBox.Items.Add($"KEY: {window.Key}, VALUE: {window.Value}");
            }
        }
    }



    internal static class Native
    {
        internal static IDictionary<HWND, string> GetOpenWindows()
        {
            var shellWindow = PInvoke.GetShellWindow();
            var windows = new Dictionary<HWND, string>();

            PInvoke.EnumWindows((hWnd, lParam) =>
            {
                if (hWnd == shellWindow) return true;
                if (!PInvoke.IsWindowVisible(hWnd)) return true;

                var length = PInvoke.GetWindowTextLength(hWnd);
                if (length == 0)
                    return true;

                var str = new StringBuilder(256);
                var result = GetWindowText(hWnd, str, str.Capacity);
                if (result != 0)
                    windows[hWnd] = str.ToString();

                return true;

            }, 0);

            return windows;
        }


        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    }


}