using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Oldseal
{
    public string Sn { get; set; } = null!;

    public string AccNo { get; set; } = null!;

    public short SetNo { get; set; }

    public short SealNo { get; set; }

    public string? ImagePath { get; set; }

    public string? SealColor { get; set; }

    public string? Location { get; set; }
}
