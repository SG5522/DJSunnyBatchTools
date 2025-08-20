using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Maccount
{
    public string AccNo { get; set; } = null!;

    public string? JoinTag { get; set; }

    public DateTime? CaseDate { get; set; }
}
