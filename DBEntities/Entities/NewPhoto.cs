using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class NewPhoto
{
    public string Idnumber { get; set; } = null!;

    public string? ImagePath { get; set; }

    public string? FeatureJson { get; set; }

    public DateTime? DateTime { get; set; }

    public string ESn { get; set; } = null!;
}
