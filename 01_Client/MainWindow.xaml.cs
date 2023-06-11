using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace _02_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UdpClient udpClient;
        IPEndPoint endPoint;
        public MainWindow()
        {
            InitializeComponent();
            udpClient = new UdpClient();
            endPoint = new IPEndPoint(IPAddress.Parse("172.20.10.2"), 3344);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // send to the server
            byte[] data = Encoding.UTF8.GetBytes(country_tb.Text);
            udpClient.Send(data, data.Length, endPoint);
            //server response
            IPEndPoint? serverEndPoint = null; 
            byte[] response = udpClient.Receive(ref serverEndPoint);
            string responseMessage = Encoding.UTF8.GetString(response);
            list.Items.Add(responseMessage);
        }
    }
}
