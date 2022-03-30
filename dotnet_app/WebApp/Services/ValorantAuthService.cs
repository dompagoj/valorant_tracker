using System.Text.Json;
using WebApp.DTOs;

namespace WebApp.Services;

public class ValorantAuthService
{
    readonly ILogger<ValorantAuthService> _logger;
    readonly IHttpClientFactory _clientFactory;
    readonly IHostEnvironment _env;
    readonly IConfiguration _configuration;

    static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    RiotTokens? Tokens { get; set; }


    public ValorantAuthService(IHttpClientFactory clientFactory, IHostEnvironment env, IConfiguration configuration,
        ILogger<ValorantAuthService> logger)
    {
        _clientFactory = clientFactory;
        _env = env;
        _configuration = configuration;
        _logger = logger;
    }

    public async ValueTask<RiotTokens> GetTokens()
    {
        if (Tokens != null && Tokens.ExpiresAt > DateTime.Now) return Tokens;

        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync(_configuration["NodeUrl"]);
        if (!response.IsSuccessStatusCode) throw new("Node service failed");

        _logger.LogInformation("Starting node token fetch process");
        _logger.LogInformation("Done");

        var tokens = await response.Content.ReadFromJsonAsync<RiotTokens>();

        Tokens = tokens ?? throw new("Failed to serialize tokens");

        return tokens;
    }
}
