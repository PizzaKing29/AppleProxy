using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AppleProxy;

class Proxy
{
    public static async Task Main()
    {
        await HttpListenRequests(); // await to not stop during loop
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
                await ReverseProxy(httpListener.GetContext(), backendUrl, clientUrl);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }

    public static async Task ReverseProxy(HttpListenerContext httpListenerContext, string backendUrl, string clientUrl)
    {

        // todo: implement http responses

        // HttpListener  == Client > Proxy
        // HttpClient == Proxy > Backend
        
        try
        {
            HttpClient httpClient = new HttpClient(); // send and recieve HTTP responses
            HttpListenerResponse httpListenerResponse = httpListenerContext.Response;
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();


            // turn URI into query, and send back info to client like status code


            var clientRequest = httpListenerContext.Request; // correct

            httpRequestMessage.Method = new HttpMethod(clientRequest.HttpMethod); // convert into HttpMethod
            httpRequestMessage.RequestUri = new Uri(backendUrl);
            httpRequestMessage.Headers = clientRequest.Headers;

            var outputStream = httpListenerResponse.OutputStream;

            await httpClient.SendAsync(httpRequestMessage); // send back request to client


            /* var header = clientRequest.Content.Headers;

            var contentType = header.ContentType; // represents media type
            var contentLength = header.ContentLength; // size of message body in bytes


            // send back the HTTP response code
            var statusCode = clientRequest.StatusCode;
            httpListenerResponse.StatusCode = (int) statusCode; */



            Console.WriteLine("Sucessfully sent back HTTP response code and the HTTP response");
            httpListenerResponse.Close(); // shutdown client
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }
}