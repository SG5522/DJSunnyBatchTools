using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Hmaincase
{
    public string ESn { get; set; } = null!;

    public string CaseSn { get; set; } = null!;

    public string? AccNo { get; set; }

    public DateTime? CaseDate { get; set; }

    public string? AccountType { get; set; }

    public string? BranchId { get; set; }

    public string? CaseStep { get; set; }

    public string? DocSn { get; set; }

    public string? NamePlace { get; set; }

    public string? JoinTag { get; set; }

    public string? Status { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? AccName { get; set; }

    public string? Subject { get; set; }
}
