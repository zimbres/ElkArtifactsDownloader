namespace ElkArtifactsDownloader;

internal class Settings
{
    public string BaseUrl { get; set; }
    public string Version { get; set; }
    public List<string> Sulfixes { get; set; }
    public List<Artifacts> Artifacts { get; set; }
}

internal class Artifacts
{
    public string FolderName { get; set; }
    public string Artifact { get; set; }
}
