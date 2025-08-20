using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Msealcard
{
    public string AccNo { get; set; } = null!;

    public DateOnly? AvailableDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public DateTime? ScanDateTime { get; set; }

    public string? PathFront { get; set; }

    public string? PathBack { get; set; }

    public string? SealCardNote { get; set; }

    public short? TotalSet { get; set; }

    public short? TotalSeals { get; set; }
}
