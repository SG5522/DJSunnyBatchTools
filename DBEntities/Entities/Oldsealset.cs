using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Oldsealset
{
    public string Sn { get; set; } = null!;

    public string AccNo { get; set; } = null!;

    public short Setno { get; set; }

    public short? Sealnum { get; set; }
}
