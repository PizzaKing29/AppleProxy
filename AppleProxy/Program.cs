using System.Net;
using System.Net.Sockets;

namespace AppleProxy;

class Proxy
{
    public static void Main()
    {
        HttpListenRequests();
    }

    public static void HttpListenRequests()
    {
        try
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:8001/");

            httpListener.Start();
            Console.WriteLine("Started listening for HTTP requests");

            while (true)
            {
                httpListener.GetContext();
                Console.WriteLine("Recieved HTTP Request");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }
}