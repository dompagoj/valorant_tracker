using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using WebApp.DTOs;
using ValorantSkinsDb = System.Collections.Generic.Dictionary<System.Guid, WebApp.DTOs.Level>;

namespace WebApp.Services;

public class ValorantSkinsDBService
{
    readonly HttpClient _httpClient;
    readonly ILogger<ValorantSkinsDBService> _logger;
    readonly IMemoryCache _memoryCache;
    readonly IWebHostEnvironment _env;
    readonly IConfiguration _configuration;

    public ValorantSkinsDBService(HttpClient httpClient, ILogger<ValorantSkinsDBService> logger,
        IMemoryCache memoryCache, IWebHostEnvironment env, IConfiguration configuration)
    {
        httpClient.BaseAddress = new("https://valorant-api.com/v1/");

        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;
        _env = env;
        _configuration = configuration;
    }

    public Task<ValorantSkinsDb> GetAllSkins() => _memoryCache.GetOrCreateAsync("SKINS_DB", GetSkinsImpl);

    async Task<ValorantSkinsDb> GetSkinsImpl(ICacheEntry entry)
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_configuration.GetValue<int>("CacheTime"));
        try
        {
            var skins = await (_env.IsDevelopment() ? GetFromFile() : GetFromApi());

            return skins.Data.SelectMany(d => d.Levels).ToDictionary(l => l.Uuid);
        }
        catch
        {
            return new();
        }
    }

    async Task<ValorantAllSkinsResponse> GetFromFile()
    {
        _logger.LogInformation("Fetching skins from file");
        var text = await File.ReadAllTextAsync("./skins.json");

        return JsonSerializer.Deserialize<ValorantAllSkinsResponse>(text)!;
    }

    async Task<ValorantAllSkinsResponse> GetFromApi()
    {
        _logger.LogInformation("Fetching skins from api");
        var response = await _httpClient.GetAsync("weapons/skins");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical("valorant-api.com failed to respond with 200, responded with {}", response.StatusCode);
            throw new HttpRequestException("Failed to fetch from api", null, response.StatusCode);
        }

        return (await response.Content.ReadFromJsonAsync<ValorantAllSkinsResponse>())!;
    }
}
