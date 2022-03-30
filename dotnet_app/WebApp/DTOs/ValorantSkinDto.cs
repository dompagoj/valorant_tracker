namespace WebApp.DTOs;

public class ValorantSkinDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime OfferStartDate { get; set; }
}

public class ValorantStoreDto
{
    public List<ValorantSkinDto> Offers { get; set; }
    public List<ValorantSkinDto> Bundle { get; set; }
}
