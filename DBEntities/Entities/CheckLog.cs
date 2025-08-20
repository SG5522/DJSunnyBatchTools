using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class CheckLog
{
    public string? Sn { get; set; }

    public DateTime? CheckTime { get; set; }

    public string? UserId { get; set; }

    public string? Type { get; set; }

    public string? BranchId { get; set; }
}
