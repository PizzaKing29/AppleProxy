using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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
                Console.WriteLine("Recieved HTTP Request");
                ReverseProxy(httpListener.GetContext());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }
    // GET request to the backend, then forward it to the client

    public static async Task ReverseProxy(HttpListenerContext httpListenerContext)
    {
        string baseUrl = "http://localhost:8000/"; // backend URL
        string clientUrl = "http://localhost:8000/"; // client URL
        HttpClient httpClient = new HttpClient(); // send and recieve HTTP responses
        HttpListenerResponse httpListenerResponse = httpListenerContext.Response;

        var request = await httpClient.GetStreamAsync(baseUrl);
        var outputStream = httpListenerResponse.OutputStream;
        await request.CopyToAsync(outputStream); // send back request to client

        Console.WriteLine("Worked");
        httpListenerResponse.Close(); // shutdown client
    }
}