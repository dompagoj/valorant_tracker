using System.Text.Json.Serialization;

namespace WebApp.DTOs;

using System;

public class ValorantStoreResponse
{
    public ValorantFeaturedBundle FeaturedBundle { get; set; }

    public SkinsPanelLayout SkinsPanelLayout { get; set; }
}

public class ValorantFeaturedBundle
{
    public ValorantStoreBundle Bundle { get; set; }

    public ValorantStoreBundle[] Bundles { get; set; }

    public long BundleRemainingDurationInSeconds { get; set; }
}

public class ValorantStoreBundle
{
    [JsonPropertyName("ID")]
    public Guid Id { get; set; }

    public ItemElement[] Items { get; set; }

    public long DurationRemainingInSeconds { get; set; }

    public bool WholesaleOnly { get; set; }
}

public class ItemElement
{
    public ItemItem Item { get; set; }

    public long BasePrice { get; set; }

    public double DiscountPercent { get; set; }

    public long DiscountedPrice { get; set; }

    public bool IsPromoItem { get; set; }
}

public class ItemItem
{
    [JsonPropertyName("ItemTypeID")]
    public Guid ItemTypeId { get; set; }

    [JsonPropertyName("ItemID")]
    public Guid ItemId { get; set; }

    public long Amount { get; set; }
}

public class SkinsPanelLayout
{
    public Guid[] SingleItemOffers { get; set; }

    public long SingleItemOffersRemainingDurationInSeconds { get; set; }
}
