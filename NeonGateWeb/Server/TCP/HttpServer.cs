using System.Net;
using System.Text;
using NeonGateWeb.Global;
using NeonGateWeb.Utils;

namespace NeonGateWeb.Server.TCP;

public static class HttpServer
{
    // server state
    private static bool _connected;
    
    /// <summary>
    /// Indicates the servers connection state.
    /// </summary>
    public static bool isServerConnected => _connected;
    
    // server components
    private static HttpListener _listener;
    private static readonly Dictionary<string, DelegatePair> _SPostRequestLookup = new();
    private static readonly Dictionary<string, DelegatePair> _SGetRequestLookup = new();
    
    /// <summary>
    /// Maps a post request to a delegate.
    /// </summary>
    /// <param name="path">The path of the post request.</param>
    /// <param name="handler">The delegate to invoke on request.</param>
    public static void MapPostRequest(string path, DelegatePair handler) =>
        _SPostRequestLookup.Add(path, handler);
    
    /// <summary>
    /// Maps a get request to a delegate.
    /// </summary>
    /// <param name="path">The path of the get request.</param>
    /// <param name="handler">The delegate to invoke on request.</param>
    public static void MapGetRequest(string path, DelegatePair handler) =>
        _SGetRequestLookup.Add(path, handler);

    /// <summary>
    /// Handles the post request.
    /// </summary>
    /// <param name="request">A post request.</param>
    /// <returns>The body of the post request.</returns>
    private static async Task<string> HandlePost(HttpListenerRequest request)
    {
       if(PostPathOrError(request, out var path)) return path; 
        using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
        
        var requestBody = await reader.ReadToEndAsync();
        if (request.QueryString.Count == 0)
            return await _SPostRequestLookup[path].Invoke(requestBody);

        var args = new string[request.QueryString.AllKeys.Length + 1];
        args[0] = requestBody;
        Array.Copy(request.QueryString.AllKeys, 0, args, 1, request.QueryString.AllKeys.Length);

        return await _SPostRequestLookup[path].Invoke(args);
    }

    /// <summary>
    /// Handles the get request.
    /// </summary>
    /// <param name="request">A get request.</param>
    /// <returns>The body of the get request.</returns>
    private static async Task<string> HandleGet(HttpListenerRequest request)
    {
        if (GetPathOrError(request, out var path)) return path;
        
        //_getRequestLookup[path].Invoke()
       if (request.QueryString.Count == 0) 
           return await _SGetRequestLookup[path].Invoke();
       return await _SGetRequestLookup[path].Invoke(request.QueryString.AllKeys);

    }

    /// <summary>
    /// Starts the http server.
    /// </summary>
    public static async Task Start(string url)
    {
        NrLogger.Log("Starting server...");
        try
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            _listener.Start();
        }
        catch(Exception ex) 
        {
            NrLogger.Log("Server failed to start.");
            NrLogger.Log(ex.Message);
            NrLogger.Log(ex.InnerException.Message);
            return;
        }
        _connected = true;
        while (_connected)
        {
            var context = await _listener.GetContextAsync();
            if (!_connected) break;
            var request = context.Request;
            var responseString = "Unrecognized request";
            try
            {
                if (request.HttpMethod == "GET") responseString = await HandleGet(context.Request);
                if (request.HttpMethod == "POST") responseString = await HandlePost(context.Request);
            }
            catch (Exception ex)
            {
                // todo, more formal error code returns
                await Respond("404", context);
                continue;
            }
            if (!_connected) break;
            await Respond(responseString, context);
        }
    }

    /// <summary>
    /// Builds the response and closes the OutputStream.
    /// </summary>
    /// <param name="responseString">The response handled by the app.</param>
    /// <param name="context">The listener's context.</param>
    private static async Task Respond(string responseString, HttpListenerContext context)
    {
        var responseBytes = Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = responseBytes.Length;
        await context.Response.OutputStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        context.Response.OutputStream.Close();
    }

    // todo, ending server throws exception because while loop
    // wrap the start in a try finally
    /// <summary>
    /// Stop the server and update it's state.
    /// </summary>
    public static void Stop()
    {
        _connected = false;
    }

    // todo, use request to get request type, and then use a dictionary to get the delegate
    // delete one of these methods.
    /// <summary>
    /// Gets the path from the post request and checks if it is valid.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="path">The path or an error message.</param>
    /// <returns>The path exists, or an error message.</returns>
    private static bool PostPathOrError(HttpListenerRequest request, out string path)
    {
        path = "Unrecognized request";
        if (request.Url == null || !_SPostRequestLookup.ContainsKey(request.Url.AbsolutePath)) 
            return true;
        path = request.Url.AbsolutePath;
        return false;
    }
    
    /// <summary>
    /// Gets the path from the get request and checks if it is valid.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="path">The path or an error message.</param>
    /// <returns>The path exists, or an error message.</returns> 
    private static bool GetPathOrError(HttpListenerRequest request, out string path)
    {
        path = "Unrecognized request";
        if (request.Url == null || !_SGetRequestLookup.ContainsKey(request.Url.AbsolutePath)) 
            return true;
        path = request.Url.AbsolutePath;
        return false;
    }
}