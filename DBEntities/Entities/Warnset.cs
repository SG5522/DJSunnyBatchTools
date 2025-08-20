using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Warnset
{
    public string AccNo { get; set; } = null!;

    public string? SealLost { get; set; }

    public string? OralLost { get; set; }

    public DateOnly? OralLostDate { get; set; }

    public TimeOnly? OralLostTime { get; set; }

    public string? OralLostUserId { get; set; }

    public string? BookLost { get; set; }

    public DateOnly? BookLostDate { get; set; }

    public TimeOnly? BookLostTime { get; set; }

    public string? BookLostUserId { get; set; }

    public string? Refusal { get; set; }

    public DateOnly? RefusalDate { get; set; }

    public TimeOnly? RefusalTime { get; set; }

    public string? BranchId { get; set; }
}
