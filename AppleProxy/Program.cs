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
                // httpListener.GetContext();
                Console.WriteLine("Recieved HTTP Request");
                ReverseProxy(httpListener.GetContext());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }


    public static void ReverseProxy(HttpListenerContext httpListenerContext)
    {
        // when program detects a http request on port 8001, GET http request stuff form port 8000
        string baseUrl = "http://localhost:8000/";
        HttpClient httpClient = new HttpClient();
        HttpListenerResponse httpListenerResponse = httpListenerContext.Response;

        var request = httpClient.GetStreamAsync(baseUrl);

        Console.WriteLine("I think it worked");
        Console.WriteLine($"Listener Context: {httpListenerContext}");
        Console.WriteLine($"Listener Response: {httpListenerResponse}");
    }
}