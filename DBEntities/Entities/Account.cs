using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Account
{
    public decimal Depkind { get; set; }

    public decimal Acno { get; set; }

    public string? Idno { get; set; }

    public string? Name { get; set; }

    public string? AccNo { get; set; }
}
