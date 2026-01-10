using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AppleProxy;

class Proxy
{
    public static async Task Main()
    {
        await HttpListenRequests();
    }

    public static async Task HttpListenRequests()
    {
        try
        {
            string backendUrl = "http://localhost:8000/"; // backend URL
            string clientUrl = "http://localhost:8001/"; // client URL


            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(clientUrl);

            httpListener.Start();
            Console.WriteLine("Started listening for HTTP requests");


            while (true)
            {
                Console.WriteLine("Recieved HTTP Request");
                await ReverseProxy(httpListener.GetContext(), backendUrl);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }

    public static async Task ReverseProxy(HttpListenerContext httpListenerContext, string backendUrl)
    {
        // HttpListener  == Client > Proxy
        // HttpClient == Proxy > Backend
        
        try
        {
            HttpClient httpClient = new HttpClient(); // send and recieve HTTP responses
            HttpListenerResponse httpListenerResponse = httpListenerContext.Response;


            var request = await httpClient.GetAsync(backendUrl);
            var outputStream = httpListenerResponse.OutputStream;
            await request.Content.CopyToAsync(outputStream); // send back request to client

            // send back the HTTP response code
            var statusCode = request.StatusCode;
            httpListenerResponse.StatusCode = (int) statusCode; 


            Console.WriteLine("Sucessfully sent back HTTP response code and the HTTP response");
            httpListenerResponse.Close(); // shutdown client
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }
}