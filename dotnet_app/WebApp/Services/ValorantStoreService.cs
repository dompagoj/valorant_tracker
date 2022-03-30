using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using WebApp.DTOs;

namespace WebApp.Services;

public class ValorantStoreService
{
    readonly IMemoryCache _memoryCache;
    readonly ValorantAuthService _valorantAuthService;
    readonly ILogger<ValorantStoreService> _logger;
    readonly IHttpClientFactory _clientFactory;
    readonly IConfiguration _configuration;

    public ValorantStoreService(IMemoryCache memoryCache, ILogger<ValorantStoreService> logger,
        ValorantAuthService valorantAuthService, IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _valorantAuthService = valorantAuthService;
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    public Task<ValorantStoreResponse> GetFromStore() => _memoryCache.GetOrCreateAsync("STORE_SKINS", GetFromStoreImpl);

    async Task<ValorantStoreResponse> GetFromStoreImpl(ICacheEntry entry)
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_configuration.GetValue<int>("StoreCacheTime"));

        var httpClient = _clientFactory.CreateClient();
        var tokens = await _valorantAuthService.GetTokens();
        httpClient.DefaultRequestHeaders.Authorization =
            AuthenticationHeaderValue.Parse($"Bearer {tokens.AccessToken}");
        httpClient.DefaultRequestHeaders.Add("X-Riot-Entitlements-JWT", tokens.EntitlementToken);

        var result =
            await httpClient.GetAsync(
                $"https://pd.eu.a.pvp.net/store/v2/storefront/{_configuration["UserUuid"]}");

        if (!result.IsSuccessStatusCode) throw new("Failed to fetch from store");

        return (await result.Content.ReadFromJsonAsync<ValorantStoreResponse>())!;
    }
}
