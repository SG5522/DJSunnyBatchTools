using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Vein
{
    public string Idnumber { get; set; } = null!;

    public string? ImageJson { get; set; }

    public DateTime? DateTime { get; set; }

    public string? FeatureJson { get; set; }
}
