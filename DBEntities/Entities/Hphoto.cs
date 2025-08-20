using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Hphoto
{
    public string? Idnumber { get; set; }

    public string? ImagePath { get; set; }

    public string? FeatureJson { get; set; }

    public DateTime? DateTime { get; set; }
}
