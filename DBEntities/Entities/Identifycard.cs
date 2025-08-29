
namespace DBEntities.Entities;

public partial class IdentifyCard
{
    public string Idnumber { get; set; } = null!;

    public string? Imagefpath { get; set; }

    public string Order { get; set; } = null!;

    public DateTime? DateTime { get; set; }

    public string? Housebook { get; set; }
}
