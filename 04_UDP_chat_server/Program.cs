// -------------- Chat Server --------------

using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

const int port = 3737;
const string JOIN_CMD = "<join>";
const string LEAVE_CMD = "<leave>";

UdpClient server = new(port);

// can not store dublicators
HashSet<IPEndPoint> members = new();

while (true)
{
    IPEndPoint clientIp = null;

    // waiting for a client message
    byte[] data = server.Receive(ref clientIp);
    string message = Encoding.UTF8.GetString(data);
    const int maxMembers = 10;

    Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] - {message} | from {clientIp}");

    if (message.StartsWith(JOIN_CMD))
    {
        if (members.Count < maxMembers)
        {
            members.Add(clientIp);
        }
        else
        {
            string Message = "The chat is full. Cannot join.";
            byte[] Data = Encoding.UTF8.GetBytes(Message);
            server.Send(Data, clientIp);
        }
    }
    else if (message.StartsWith(LEAVE_CMD))
    {
        members.Remove(clientIp);
    }
    else if (members.Contains(clientIp))
    {
        foreach (IPEndPoint ip in members)
        {
          server.Send(data, data.Length, ip);
        }
    }

    /*switch (message)
     {
         case JOIN_CMD:
             if (members.Count < maxMembers)
             {
                 members.Add(clientIp);
             }
             else
             {
                 string Message = "The chat is full. Cannot join.";
                 byte[] Data = Encoding.UTF8.GetBytes(Message);
                 server.Send(Data,clientIp);
             }
             break;
         case LEAVE_CMD:
             members.Remove(clientIp);
             break;
         default:
             foreach (IPEndPoint ip in members)
                 server.Send(data, ip);
             break;
     }*/
}
