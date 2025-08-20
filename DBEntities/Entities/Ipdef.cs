using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Ipdef
{
    public string Ip { get; set; } = null!;

    public decimal? Xcoordinate { get; set; }

    public decimal? Ycoordinate { get; set; }

    public string? EyesComparison { get; set; }

    public string? SealComparison { get; set; }

    public string? Opendesk { get; set; }
}
