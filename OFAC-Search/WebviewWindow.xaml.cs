using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows;

namespace OFAC_Search
{
    /// <summary>
    /// Interaction logic for WebviewWindow.xaml
    /// </summary>
    public partial class WebviewWindow : Window
    {
        string Folder = string.Empty;
        public WebviewWindow(string folder)
        {
            InitializeComponent();
            Folder = folder;
        }

        public async void init()
        {
            await webView.EnsureCoreWebView2Async();
            webView.Source = new Uri("https://sanctionssearch.ofac.treas.gov");
            webView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        }//

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
        }

        private void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            var dirName = new DirectoryInfo(Folder).Name;
            webView.CoreWebView2.DOMContentLoaded -= CoreWebView2_DOMContentLoaded;
            webView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded2;
            var script = @"var compName = document.getElementById(""ctl00_MainContent_txtLastName"");" +
                        $"compName.value= '{dirName}';" +
                        @"var searchBtn = document.getElementById(""ctl00_MainContent_btnSearch"");
                        searchBtn.click();";
            webView.CoreWebView2.ExecuteScriptAsync(script);
        }//

        private async void CoreWebView2_DOMContentLoaded2(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            var printSettings = webView.CoreWebView2.Environment.CreatePrintSettings();

            await webView.CoreWebView2.PrintToPdfAsync(Path.Combine(Folder, $"OFAC {DateTime.Now.ToString("yyyy-MM-dd")}.pdf"), printSettings);
            this.Close();
        }
    }
}
