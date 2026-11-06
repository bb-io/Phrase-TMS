using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PhraseTMSTests.Base;

public sealed class FakePhraseServer : IDisposable
{
    private readonly HttpListener _listener = new();
    private readonly Func<HttpListenerRequest, string> _respond;
    private readonly CancellationTokenSource _stop = new();

    public string Url { get; }
    public int AsyncStatusRequestCount { get; private set; }

    public FakePhraseServer(Func<HttpListenerRequest, string> respond)
    {
        _respond = respond;
        Url = $"http://localhost:{GetFreePort()}/";
        _listener.Prefixes.Add(Url);
        _listener.Start();
        _ = Task.Run(ServeAsync);
    }

    public static string PendingAsyncRequestJson(string id) =>
        $"{{\"id\":\"{id}\",\"action\":\"EXPORT_TM\",\"dateCreated\":\"2026-01-01T00:00:00+0000\",\"asyncResponse\":null}}";

    public static string CompletedAsyncRequestJson(string id) =>
        $"{{\"id\":\"{id}\",\"action\":\"EXPORT_TM\",\"dateCreated\":\"2026-01-01T00:00:00+0000\",\"asyncResponse\":{{\"dateCreated\":\"2026-01-01T00:00:01+0000\"}}}}";

    private async Task ServeAsync()
    {
        while (!_stop.IsCancellationRequested)
        {
            HttpListenerContext context;
            try
            {
                context = await _listener.GetContextAsync();
            }
            catch (Exception) when (_stop.IsCancellationRequested)
            {
                return;
            }

            if (context.Request.Url!.AbsolutePath.Contains("/api2/v1/async/"))
                AsyncStatusRequestCount++;

            var body = Encoding.UTF8.GetBytes(_respond(context.Request));
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.OutputStream.WriteAsync(body);
            context.Response.Close();
        }
    }

    private static int GetFreePort()
    {
        var socket = new TcpListener(IPAddress.Loopback, 0);
        socket.Start();
        var port = ((IPEndPoint)socket.LocalEndpoint).Port;
        socket.Stop();
        return port;
    }

    public void Dispose()
    {
        _stop.Cancel();
        _listener.Close();
    }
}
