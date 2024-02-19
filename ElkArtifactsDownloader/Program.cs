using ElkArtifactsDownloader;
using System.Net;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<Worker>();
        services.AddHttpClient();
        services.AddHttpClient("Proxy", client => { })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var localSettings = hostContext.Configuration.GetSection("LocalSettings");
                return new HttpClientHandler
                {
                    Proxy = new WebProxy(localSettings["ProxyAddress"] + ":" + localSettings["ProxyPort"]),
                    UseProxy = true
                };
            });
    })
    .Build();

await host.Services.GetRequiredService<Worker>().ExecuteAsync();
