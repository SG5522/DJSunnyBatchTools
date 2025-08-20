using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Oldvein
{
    public string? Idnumber { get; set; }

    public string? ImageJson { get; set; }

    public DateTime? DateTime { get; set; }

    public string? FeatureJson { get; set; }

    public string Sn { get; set; } = null!;
}
