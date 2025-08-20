using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Hsealset
{
    public string AccNo { get; set; } = null!;

    public short Setno { get; set; }

    public short? Sealnum { get; set; }
}
