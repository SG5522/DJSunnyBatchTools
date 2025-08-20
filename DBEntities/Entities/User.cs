using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? BranchId { get; set; }

    public string? UserName { get; set; }

    public string? StaffLevel { get; set; }

    public string? UserPass { get; set; }

    public string? Status { get; set; }

    public string? Permission { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Remark { get; set; }
}
