
namespace WebApp.DTOs;

using System;

public class ValorantAllSkinsResponse
{
    public long Status { get; set; }

    public ValorantSkinData[] Data { get; set; }
}

public class ValorantSkinData
{
    public Guid Uuid { get; set; }

    public string DisplayName { get; set; }

    public Guid ThemeUuid { get; set; }

    public Guid? ContentTierUuid { get; set; }

    public Uri DisplayIcon { get; set; }

    public Uri Wallpaper { get; set; }

    public string AssetPath { get; set; }

    public Chroma[] Chromas { get; set; }

    public Level[] Levels { get; set; }
}

public class Chroma
{
    public Guid Uuid { get; set; }

    public string DisplayName { get; set; }

    public Uri? DisplayIcon { get; set; }

    public Uri FullRender { get; set; }

    public Uri Swatch { get; set; }

    public Uri StreamedVideo { get; set; }

    public string AssetPath { get; set; }
}

public class Level
{
    public Guid Uuid { get; set; }

    public string DisplayName { get; set; }

    public string? LevelItem { get; set; }

    public Uri? DisplayIcon { get; set; }

    public Uri StreamedVideo { get; set; }

    public string AssetPath { get; set; }
}





