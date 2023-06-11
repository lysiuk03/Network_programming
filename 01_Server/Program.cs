

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        // ...\01_Server\bin\Debug\net7.0\Country-Capital.json
        string fileName = "Country-Capital.json";
        string jsonString = File.ReadAllText(fileName);
        Dictionary<string, string>? CountryCapital = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("172.20.10.2"), 3344);
        UdpClient server = new UdpClient(endPoint);
        while(true)
        {
            IPEndPoint? clientEndPoint = null;
            byte[] request = server.Receive(ref clientEndPoint);

            string country = Encoding.UTF8.GetString(request);
            Console.WriteLine($"Country: {country} from {clientEndPoint}");

            string data="Country not found!";
            if (CountryCapital?.GetValueOrDefault(country)!=null)
            {
                data = country + ": " + CountryCapital.GetValueOrDefault(country);
            }
            byte[] response = Encoding.UTF8.GetBytes(data);
            server.Send(response, response.Length, clientEndPoint);
        }
    }
}