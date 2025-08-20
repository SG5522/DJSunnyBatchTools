using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class AccType
{
    public string Subno { get; set; } = null!;

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Check { get; set; }
}
