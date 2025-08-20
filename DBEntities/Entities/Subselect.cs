using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Subselect
{
    public string ESn { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string CaseSn { get; set; } = null!;

    public string? AccNo { get; set; }

    public string? ApplicationItem { get; set; }
}
