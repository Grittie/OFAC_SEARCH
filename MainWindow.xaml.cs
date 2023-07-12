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
        }

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

            foreach (var folder in folders)
            {
                resVal.Text = $"{++index} / {folders.Count}";
                var webView = new WebviewWindow(folder);
                webView.init();
                webView.ShowDialog();
            }//

            StartBtn.IsEnabled = true;
            OpenSource.IsEnabled = true;
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
