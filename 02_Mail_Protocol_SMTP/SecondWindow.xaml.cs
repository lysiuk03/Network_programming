using System.Windows;
using System.Windows.Controls;

namespace _02_Mail_Protocol_SMTP
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string SmtpServerAddress { get; set; }
        public int SmtpPort { get; set; }

        public SecondWindow()
        {
            InitializeComponent();
            login.Text = "lysiuk03";
            password.Password = "ecnmiijsfdfexfqc";
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(login.Text) && !string.IsNullOrEmpty(password.Password))
            {
                Login = login.Text+ ((ComboBoxItem)provider.SelectedItem).Content.ToString();
                Password = password.Password;
                SmtpPort = 465;
                switch (((ComboBoxItem)provider.SelectedItem).Content.ToString())
                {
                    case "@gmail.com":
                        SmtpServerAddress = "smtp.gmail.com";
                        break;
                    case "@ukr.net":
                        SmtpServerAddress = "smtp.ukr.net";
                        break;
                    case "@outlook.com":
                        SmtpServerAddress = "smtp.office365.com";
                        break;
                    default:
                        break;
                }
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("You have not entered all the data!");
            }
        }

    }
}
