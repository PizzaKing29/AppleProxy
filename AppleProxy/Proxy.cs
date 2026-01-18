using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AppleProxy;

class Proxy
{
    private const string BackendUrl = "http://localhost:8000/";
    private const string ClientUrl = "http://localhost:8001/";

    public static async Task StartProxy()
    {

        try
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(ClientUrl);

            httpListener.Start();
            Console.WriteLine("Started listening for HTTP requests");


            while (true)
            {
                Console.WriteLine("Recieved HTTP Request");
                await ReverseProxy(httpListener.GetContext());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error {e}");
        }
    }

    public static async Task ReverseProxy(HttpListenerContext httpListenerContext)
    {

        // todo: implement http responses
        // add port changing
        // all http requests

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
            httpRequestMessage.RequestUri = new Uri(BackendUrl);


            var setHeader = httpRequestMessage;
            foreach (var header in clientRequest.Headers.AllKeys)
            {
                var value = clientRequest.Headers[header];

                switch (header)
                {
                    case "Host":
                        setHeader.Headers.Host = value;
                        break;

                    case "User-Agent":
                        setHeader.Headers.Add("User-Agent", value);
                        break;

                    case "Accept":
                        setHeader.Headers.Add("Accept", value);
                        break;

                    case "Accept-Encoding":
                        setHeader.Headers.Add("Accept-Encoding", value);
                        break;

                    case "Cookie":
                        setHeader.Headers.Add("Cookie", value);
                        break;

                    case "Authorization":
                        setHeader.Headers.Add("Authorization", value);
                        break;

                    case "Content-Type":
                        setHeader.Headers.Add("Content-Type", value);
                        break;

                    case "Content-Length":
                        setHeader.Headers.Add("Content-Length", value);
                        break;

                    case "Referer":
                        setHeader.Headers.Add("Referer", value);
                        break;
                }
            }

            // httpRequestMessage.Headers = clientRequest.Headers;

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