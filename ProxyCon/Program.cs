using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MihaZupan; // For SOCKS4 and SOCKS5 support

class Program
{
    private static HttpClient httpClient; // Shared HttpClient instance
    private static bool usingProxy = false; // To track proxy usage

    static async Task Main(string[] args)
    {
        Console.Clear();
        DrawSplash();

        while (true)
        {
            Console.WriteLine("    +────────────+─────────────────────────────────────────────────────────────────────+");
            Console.WriteLine("    │ Option     │ Description                                                         │");
            Console.WriteLine("    +────────────+─────────────────────────────────────────────────────────────────────+");
            Console.WriteLine("    │ 1.         │ Connect to a proxy server (HTTP, HTTPS, SOCKS4, SOCKS5).            │");
            Console.WriteLine("    │ 2.         │ Revert proxy settings, clean traces and disconnect from the proxy.  │");
            Console.WriteLine("    │ 3.         │ Exit the program.                                                   │");
            Console.WriteLine("    +────────────+─────────────────────────────────────────────────────────────────────+");
            Console.WriteLine();
            Console.Write("    $ ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ConnectToProxyAsync();
                    break;

                case "2":
                    RevertProxy();
                    break;

                case "3":
                    Console.WriteLine("    $ Goodbye...");
                    return;

                default:
                    Console.WriteLine("    $ Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DrawSplash()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(@"                                     
 _____                 _____         
|  _  |___ ___ _ _ _ _|     |___ ___ 
|   __|  _| . |_'_| | |   --| . |   |
|__|  |_| |___|_,_|_  |_____|___|_|_|
                  |___|              ");
        Console.ResetColor();
        Console.Write(@"  Proxy connection services.");
        Console.WriteLine();
        Console.WriteLine();
        Console.ResetColor();
    }

    static async Task ConnectToProxyAsync()
    {   
        Console.Clear();
        DrawSplash();

        Console.Write("    $ Enter proxy type (http, https, socks4, socks5): ");
        string proxyType = Console.ReadLine()?.ToLower();

        Console.Write("    $ Enter proxy address (e.g., 127.0.0.1:1080): ");
        string proxyAddress = Console.ReadLine();

        Console.Write("    $ Enter proxy username (leave blank if not needed): ");
        string proxyUsername = Console.ReadLine();

        Console.Write("    $ Enter proxy password (leave blank if not needed): ");
        string proxyPassword = Console.ReadLine();

        Console.Write("    $ Enter the URL to fetch: ");
        string url = Console.ReadLine();

        try
        {
            // Parse the proxy address into host and port
            (string host, int port) = ParseProxyAddress(proxyAddress);

            // Create a new HttpClient instance configured with the proxy
            httpClient = CreateHttpClient(proxyType, host, port, proxyUsername, proxyPassword);
            usingProxy = true;

            Console.WriteLine("    $ Sending request through the proxy...");
            var response = await httpClient.GetAsync(url);

            Console.WriteLine($"    $ Response Status: {response.StatusCode}");
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("    $ Response Content:");
            Console.WriteLine(content[..Math.Min(500, content.Length)]); // Show first 500 characters
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    $ {ex.Message}\n");
        }
    }

    static void RevertProxy()
    {
        if (!usingProxy)
        {
            Console.WriteLine("You are not connected to any proxy.\n");
            return;
        }

        // Dispose of the current HttpClient instance
        httpClient?.Dispose();
        httpClient = new HttpClient(); // Create a new HttpClient without proxy
        usingProxy = false;

        Console.WriteLine("    $ Reverted to default network settings. Proxy disconnected.");
    }

    static HttpClient CreateHttpClient(string proxyType, string host, int port, string proxyUsername, string proxyPassword)
    {
        HttpClient httpClient;
        switch (proxyType)
        {
            case "http":
            case "https":
                // Use HttpClientHandler for HTTP/HTTPS proxies
                var httpProxy = new WebProxy($"{host}:{port}")
                {
                    UseDefaultCredentials = false,
                    Credentials = string.IsNullOrWhiteSpace(proxyUsername)
                        ? null
                        : new NetworkCredential(proxyUsername, proxyPassword)
                };

                var httpHandler = new HttpClientHandler
                {
                    Proxy = httpProxy,
                    UseProxy = true
                };

                httpClient = new HttpClient(httpHandler);
                break;

            case "socks4":
            case "socks5":
                // Use MihaZupan.HttpToSocks5Proxy for SOCKS proxies
                var socksProxy = new HttpToSocks5Proxy(host, port, proxyUsername, proxyPassword);

                var socketsHandler = new SocketsHttpHandler
                {
                    Proxy = socksProxy, // Set the proxy
                    UseProxy = true
                };

                httpClient = new HttpClient(socketsHandler);
                break;

            default:
                throw new ArgumentException($"Unsupported proxy type: {proxyType}\n");
        }

        return httpClient;
    }

    static (string host, int port) ParseProxyAddress(string proxyAddress)
    {
        if (string.IsNullOrWhiteSpace(proxyAddress))
        {
            throw new ArgumentException("Proxy address cannot be empty.");
        }

        var parts = proxyAddress.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int port))
        {
            throw new ArgumentException("Invalid proxy address format. Expected format: host:port");
        }

        return (parts[0], port);
    }
}
