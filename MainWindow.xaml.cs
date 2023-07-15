using System.IO;
using System.Linq;
using System.Windows;

namespace OFAC_Search
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }//

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DirSrc.Text))
            {
                MessageBox.Show("Directory can't be empty");
                return;
            }
            else if (!Directory.Exists(DirSrc.Text))
            {
                MessageBox.Show("Directory not found");
                return;
            }

            StartBtn.IsEnabled = false;
            OpenSource.IsEnabled = false;
            resVal.Text = string.Empty;

            var folders = Directory.GetDirectories(DirSrc.Text, "*", SearchOption.AllDirectories).ToList();
            folders.Insert(0, DirSrc.Text);
            int index = 0;

            string firstFolder = null;

            foreach (var folder in folders)
            {
                if (firstFolder == null)
                {
                    firstFolder = folder;
                }

                resVal.Text = $"{++index} / {folders.Count}";
                var webView = new WebviewWindow(folder, firstFolder);
                webView.init();
                webView.ShowDialog();
            }

            // Use the firstFolder variable here as needed

            StartBtn.IsEnabled = true;
            OpenSource.IsEnabled = true;
        }//


        private void DifferentFunction(string folder)
        {
            // Perform operations with the first found folder
            // Use the 'folder' variable here
        }

        private void OpenSource_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    DirSrc.Text = fbd.SelectedPath;
            }
        }//
    }
}
