using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Remark
{
    public string Sn { get; set; } = null!;

    public string? AccNo { get; set; }

    public string? PhotoPath { get; set; }

    public string? Idfpath { get; set; }

    public string? Idbpath { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public string? BranchId { get; set; }
}
