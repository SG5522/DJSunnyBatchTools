using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Docset
{
    public string AccNo { get; set; } = null!;

    public string? SetOrder { get; set; }

    public DateTime? Createtime { get; set; }
}
