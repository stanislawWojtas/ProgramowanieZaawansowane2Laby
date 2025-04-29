namespace linq;

public class Territory{
    public string? TerritoryId;
    public string? TerritoryDescription;
    public string? RegionId;
    public Territory(string? territoryId, string? territoryDescription, string? regionId)
    {
        TerritoryId = territoryId;
        TerritoryDescription = territoryDescription;
        RegionId = regionId;
    }
}