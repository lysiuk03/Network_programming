using System.Windows;

namespace _02_Mail_Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string SmtpServerAddress { get; set; }
        public int SmtpPort { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(login.Text) && !string.IsNullOrEmpty(password.Password))
            {
                Login = login.Text;
                Password = password.Password;
                switch (provider.SelectedItem.ToString())
                {
                    case "@gmail.com":
                        SmtpServerAddress = "smtp.gmail.com";
                        SmtpPort = 587;
                        break;
                    case "@ukr.net":
                        SmtpServerAddress = "smtp.ukr.net";
                        SmtpPort = 465;
                        break;
                    case "@outlook.com":
                        SmtpServerAddress = "smtp.office365.com";
                        SmtpPort = 587;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("You have not entered all the data!");
            }
        }
    }
}
