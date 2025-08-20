using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Subject
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? No { get; set; }

    public string? CustomerList { get; set; }

    public string? Item { get; set; }
}
