using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Win32;
using MimeKit;
using MimeKit.IO;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace _02_Mail_Protocol_SMTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SmtpClient Client;
        public string Login { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            IsEnableds(false);
        }
       
        private void inbtn_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow form = new SecondWindow();
            if (form.ShowDialog() == true)
            {
                Login = form.Login;
                Client = new SmtpClient();
                try
                {
                    Client.Connect(form.SmtpServerAddress, form.SmtpPort, SecureSocketOptions.SslOnConnect);
                    Client.Authenticate(form.Login, form.Password);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eror: " + ex.Message);
                    return;
                }
                inbtn.IsEnabled = false;
                outbtn.IsEnabled = true;
                IsEnableds(true);
            }
            else
            {
                MessageBox.Show("Eror!");
            }
        }

        private void sendbtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(totb.Text))
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Vika", Login));
                message.To.Add(new MailboxAddress("Test User", totb.Text));
                message.Subject = subjecttb.Text;

                switch (((ComboBoxItem)priority.SelectedItem).Content.ToString())
                {
                    case "Low":
                        message.Importance = MessageImportance.Low;
                        break;
                    case "Normal":
                        message.Importance = MessageImportance.Normal;
                        break;
                    case "High":
                        message.Importance = MessageImportance.High;
                        break;
                    default:
                        break;
                }

                var multipart = new Multipart("mixed");

                foreach (var filePath in filelist.Items)
                {
                    var filePart = new MimePart()
                    {
                        Content = new MimeContent(File.OpenRead(filePath.ToString())),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(filePath.ToString())
                    };

                    multipart.Add(filePart);
                }

                var textPart = new TextPart("plain")
                {
                    Text = contenttb.Text
                };

                multipart.Add(textPart);
                message.Body = multipart;

                Client.Send(message);
                MessageBoxResult result = MessageBox.Show("Sent! Do you want to clear the window?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Clear();
                }
            }
            else
            {
                MessageBox.Show("Enter recipient's mail!");
            }
        }

        private void outbtn_Click(object sender, RoutedEventArgs e)
        {
            if (Client.IsConnected)
            {
                Client.Disconnect(true);
            }
            inbtn.IsEnabled = true;
            outbtn.IsEnabled = false;
            IsEnableds(false);
        }

        private void attachbtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                if (!filelist.Items.Contains(filePath))
                {
                    filelist.Items.Add(filePath);
                    deletebtn.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("This file is already in the list", "Message");
                }
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (filelist.SelectedItem != null)
            {
                filelist.Items.Remove(filelist.SelectedItem);
            }

            if (filelist.Items.Count == 0)
            {
                deletebtn.IsEnabled = false;
            }
        }
        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (Client != null)
            {
                if (Client.IsConnected)
                {
                    Client.Disconnect(true);
                }
            }
        }
        void IsEnableds(bool enabled)
        {
            totb.IsEnabled = enabled;
            subjecttb.IsEnabled = enabled;
            contenttb.IsEnabled = enabled;
            outbtn.IsEnabled = enabled;
            sendbtn.IsEnabled = enabled;
            attachbtn.IsEnabled = enabled;
            deletebtn.IsEnabled = enabled;
            priority.IsEnabled = enabled;
        }
        void Clear()
        {
            totb.Clear();
            subjecttb.Clear();
            contenttb.Clear();
            filelist.Items.Clear();
        }
    }
}
