namespace ElkArtifactsDownloader;

public class Worker
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient("Proxy");
    }

    public async Task ExecuteAsync()
    {
        var settings = _configuration.GetSection("Settings").Get<Settings>();

        foreach (var artifact in settings.Artifacts)
        {
            foreach (var sulfix in settings.Sulfixes)
            {
                await Execute(settings, artifact, sulfix);
            }
        }
    }

    private async Task Execute(Settings settings, Artifacts artifact, string sulfix)
    {
        try
        {
            var url = $"{settings.BaseUrl}{artifact.FolderName}{artifact.Artifact}-{settings.Version}-{sulfix}";

            var stream = await _httpClient.GetStreamAsync(url);

            Directory.CreateDirectory(artifact.FolderName);

            using var fileStream = new FileStream($"{artifact.FolderName}{artifact.Artifact}-{settings.Version}-{sulfix}", FileMode.Create);

            await stream.CopyToAsync(fileStream);
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex.Message);
        }
    }
}
