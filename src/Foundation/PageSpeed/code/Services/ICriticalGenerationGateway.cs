namespace Site.Foundation.PageSpeed.Repositories
{
    public interface ICriticalGenerationGateway
    {
        string GenerateCritical(string url, string width, string height);
    }
}