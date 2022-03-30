using Microsoft.Extensions.Caching.Memory;
using WebApp.DTOs;

namespace WebApp.Services;

public class ValorantService
{
    readonly ValorantSkinsDBService _skinsDBService;
    readonly ValorantStoreService _storeService;
    readonly IMemoryCache _memoryCache;
    readonly IConfiguration _configuration;

    public ValorantService(ValorantSkinsDBService skinsDBService, ValorantStoreService storeService,
        IMemoryCache memoryCache, IConfiguration configuration)
    {
        _skinsDBService = skinsDBService;
        _storeService = storeService;
        _memoryCache = memoryCache;
        _configuration = configuration;
    }

    public Task<ValorantStoreDto> GetSkins() =>
        _memoryCache.GetOrCreateAsync("SKINS", GetSkinsImpl);

    async Task<ValorantStoreDto> GetSkinsImpl(ICacheEntry entry)
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_configuration.GetValue<int>("CacheTime"));

        var allSkins = await _skinsDBService.GetAllSkins();
        var storeSkins = await _storeService.GetFromStore();

        var dto = new ValorantStoreDto();

        dto.Offers = storeSkins.SkinsPanelLayout.SingleItemOffers.Aggregate(new List<ValorantSkinDto>(),
            (acc, offerId) =>
            {
                var item = allSkins.GetValueOrDefault(offerId);
                if (item == null) return acc;

                acc.Add(new()
                {
                    Id = item.Uuid,
                    Name = item.DisplayName,
                    ImageUrl = item.DisplayIcon?.ToString(),
                });

                return acc;
            });

        dto.Bundle = storeSkins.FeaturedBundle.Bundle.Items.Aggregate(new List<ValorantSkinDto>(),
            (acc, item) =>
            {
                var foundItem = allSkins.GetValueOrDefault(item.Item.ItemId);
                if (foundItem == null) return acc;

                acc.Add(new()
                {
                    Id = foundItem.Uuid,
                    Name = foundItem.DisplayName,
                    ImageUrl = foundItem.DisplayIcon?.ToString(),
                });

                return acc;
            });

        return dto;
    }
}
