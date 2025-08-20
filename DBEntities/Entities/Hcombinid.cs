using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Hcombinid
{
    public string Idnumber { get; set; } = null!;

    public string AccNo { get; set; } = null!;

    public string? BranchId { get; set; }
}
