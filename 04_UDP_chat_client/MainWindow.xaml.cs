using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Shapes;

namespace _04_UDP_chat_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UdpClient client = new();
        private bool isListening = false;
        const string JOIN_CMD = "<join>";
        const string LEAVE_CMD = "<leave>";
        string Name="noname";
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private async void Listen()
        {
            while (isListening)
            {
                var res = await client.ReceiveAsync();
                string message = Encoding.UTF8.GetString(res.Buffer);
                string detailMessage = $"[{DateTime.Now.ToLongTimeString()}] {message}";
                msgList.Items.Add(detailMessage);
            }
        }

        private void SendMessageBtnClick(object sender, RoutedEventArgs e)
        {
            string message = msgTxtBox.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (IsCommand(message))
                {
                    MessageBox.Show("Cannot send a command as a message.", "Invalid Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SendMessage($"{Name}: {message}");
                }
            }
            else
            {
                MessageBox.Show("Cannot send an empty message.", "Invalid Message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool IsCommand(string message)
        {
            return message.Equals(JOIN_CMD, StringComparison.OrdinalIgnoreCase) || message.Equals(LEAVE_CMD, StringComparison.OrdinalIgnoreCase);
        }
        private void JoinBtnClick(object sender, RoutedEventArgs e)
        {
            Name = name.Text;
            SendMessage(JOIN_CMD);
            isListening = true;
            Listen();
            string joinMessage = $"{Name}: A member has joined the chat.";
            SendMessage(joinMessage);
        }

        private void LeaveBtnClick(object sender, RoutedEventArgs e)
        {
            string leaveMessage = $"{Name}: A member has left the chat.";
            SendMessage(leaveMessage);
            SendMessage(LEAVE_CMD);
            isListening = false;
        }

        private async void SendMessage(string text)
        {
            IPEndPoint serverIp = new IPEndPoint(IPAddress.Parse(ipTxtBox.Text), int.Parse(portTxtBox.Text));

            byte[] data = Encoding.UTF8.GetBytes(text);


            await client.SendAsync(data, serverIp);
        }
    }
}
