namespace Site.Foundation.PageSpeed.Repositories
{
    public interface ICriticalGenerationGateway
    {
        string GenerateCritical(string url, string width = "1800", string height = "1200");
    }
}