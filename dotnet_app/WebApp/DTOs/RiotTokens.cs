namespace WebApp.DTOs;

public class RiotTokens
{
    public string AccessToken { get; init; }
    public string EntitlementToken { get; set; }
    public DateTime ExpiresAt { get; init; } = DateTime.Now + TimeSpan.FromSeconds(3600);

    public RiotTokens(string accessToken, string entToken)
    {
        AccessToken = accessToken;
        EntitlementToken = entToken;
    }

    public RiotTokens() { }
}
