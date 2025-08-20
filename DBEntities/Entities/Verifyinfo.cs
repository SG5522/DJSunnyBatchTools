using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Verifyinfo
{
    public string VerifySn { get; set; } = null!;

    public string BranchId { get; set; } = null!;

    public string? ESn { get; set; }

    public string? CaseSn { get; set; }

    public string? JobCode { get; set; }

    public string? Idnumber { get; set; }

    public string? Value { get; set; }

    public string? VerifyStatus { get; set; }

    public string? Note { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public DateTime? DateTime { get; set; }

    public string? CheckUpdate { get; set; }
}
