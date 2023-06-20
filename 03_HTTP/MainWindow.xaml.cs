using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace _03_HTTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string FolderPath { get; set; }
        WebClient client = new WebClient();
        public MainWindow()
        {
            InitializeComponent();
            FolderPath= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private async void Donwload_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (client.IsBusy)
            {
                MessageBox.Show("Try again later!");
                return;
            }
            if (!string.IsNullOrEmpty(width.Text) && !string.IsNullOrEmpty(height.Text))
            {
                string url = $"https://source.unsplash.com/random/{width.Text}x{height.Text}/?{((ComboBoxItem)category.SelectedItem).Content.ToString()}&1";
                await DownloadImageAsync(url);
            }
            else
            {
                MessageBox.Show("Enter width and height!");
            }
        }
        private async Task DownloadImageAsync(string url)
        {
              string name = Path.GetRandomFileName();
              name = Path.ChangeExtension(name, "jpg");
              string dest = Path.Combine(FolderPath, name);
             await client.DownloadFileTaskAsync(url, dest);
           image.Source=new BitmapImage(new Uri(dest));
        }

        private void folder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath = dialog.FileName;
            }
        }
    }
}
