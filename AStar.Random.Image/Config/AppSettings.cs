namespace AStar.Random.Image.Config;

public class AppSettings
{
    public Logging Logging { get; set; } = new();
    public string AllowedHosts { get; set; } = string.Empty;
    public ConnectionStrings ConnectionStrings { get; set; } = new();
}
