using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Combinid
{
    public string IDNumber { get; set; } = null!;

    public string AccNo { get; set; } = null!;

    public string? BranchId { get; set; }
}
