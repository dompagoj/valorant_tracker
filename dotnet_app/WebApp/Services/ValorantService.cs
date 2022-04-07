using Microsoft.Extensions.Caching.Memory;
using WebApp.DTOs;

namespace WebApp.Services;

public interface IValorantService
{
    ValueTask<ValorantStoreDto> GetSkins();
}

public class ValorantService : IValorantService
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

    public async ValueTask<ValorantStoreDto> GetSkins()
    {
        var found = _memoryCache.Get<ValorantStoreDto>("SKINS");

        if (found != null) return found;

        var store = await GetSkinsImpl();

        _memoryCache.Set("SKINS", store, TimeSpan.FromHours(_configuration.GetValue<int>("CacheTime")));

        return store;
    }

    async Task<ValorantStoreDto> GetSkinsImpl()
    {
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

public class ValorantFakeDataService : IValorantService
{
    public ValueTask<ValorantStoreDto> GetSkins()
    {
        var image =
            "https://media.valorant-api.com/weaponskinlevels/549b06bb-4704-25ce-19d5-c9b70b10de19/displayicon.png";

        var skins = Enumerable.Range(0, 4).Select(i => new ValorantSkinDto()
        {
            Name = $"Fake Skin {i +  1}", ImageUrl = image, OfferStartDate = DateTime.Now
        }).ToList();


        return ValueTask.FromResult(new ValorantStoreDto()
        {
            Bundle = skins,
            Offers = skins,
        });
    }
}
