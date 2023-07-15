﻿using Microsoft.Web.WebView2.Core;
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
        string FirstFolder = string.Empty;
        string csvFilePath = string.Empty;

        public WebviewWindow(string folder, string firstFolder)
        {
            InitializeComponent();
            Folder = folder;
            FirstFolder = firstFolder;
            csvFilePath = firstFolder + "/results.csv";
        }

        public async void init()
        {
            await webView.EnsureCoreWebView2Async();
            webView.Source = new Uri("https://sanctionssearch.ofac.treas.gov");
            webView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
        }

        private void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            var dirName = new DirectoryInfo(Folder).Name;
            var dirFirst = new DirectoryInfo(FirstFolder).Name;

            if (dirName.Length > 1 && dirName != dirFirst)
            {
                webView.CoreWebView2.DOMContentLoaded -= CoreWebView2_DOMContentLoaded;
                webView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded2;
                var script = @"var compName = document.getElementById(""ctl00_MainContent_txtLastName"");" +
                            $"compName.value= '{dirName}';" +
                            @"var searchBtn = document.getElementById(""ctl00_MainContent_btnSearch"");
                    searchBtn.click();";
                webView.CoreWebView2.ExecuteScriptAsync(script);
            }
            else
            {
                this.Close();
            }
        }

        private async void CoreWebView2_DOMContentLoaded2(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            var printSettings = webView.CoreWebView2.Environment.CreatePrintSettings();
            string pageContent = await webView.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");

            if (pageContent.Contains("Your search has not returned any results."))
            {
                // Log negative result
                LogResult("negative");
            }
            else
            {
                // Log positive result
                LogResult("positive");
            }

            await webView.CoreWebView2.PrintToPdfAsync(Path.Combine(Folder, $"OFAC {DateTime.Now:yyyy-MM-dd}.pdf"), printSettings);
            this.Close();
        }

        void LogResult(string result)
        {
            // Get the directory name
            string dirName = new DirectoryInfo(Folder).Name;

            // Get the relative directory path
            string relativeDirPath = Path.GetRelativePath(FirstFolder, Folder);

            // Create a new line of CSV data with the directory name and relative path
            string csvLine = $"{DateTime.Now:yyyy-MM-dd},{DateTime.Now:HH:mm:ss}," +
                $"\"{relativeDirPath.Replace("\"", "\"\"")}\",\"{dirName.Replace("\"", "\"\"")}\",{result}";

            // Check if the CSV file exists
            bool csvFileExists = File.Exists(csvFilePath);

            // Open the CSV file in append mode
            using (StreamWriter sw = new StreamWriter(csvFilePath, true))
            {
                // If the file doesn't exist, add a header row
                if (!csvFileExists)
                {
                    sw.WriteLine("Date,Time,Directory,Name,Result");
                }

                // Write the line of CSV data
                sw.WriteLine(csvLine);
            }
        }
    }
}
