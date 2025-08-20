using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class OldIdentifycard
{
    public string Idnumber { get; set; } = null!;

    public string Imagefpath { get; set; } = null!;

    public string Order { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public string? Housebook { get; set; }

    public string Sn { get; set; } = null!;
}
