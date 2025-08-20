namespace DBEntities.Entities;

public partial class OldPhoto
{
    public string Idnumber { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public string? FeatureJson { get; set; }

    public DateTime DateTime { get; set; }

    public string Sn { get; set; } = null!;
}
