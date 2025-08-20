using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Customerdata
{
    public string Idnumber { get; set; } = null!;

    public string? Name { get; set; }

    public string? Sex { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Bplace { get; set; }

    public string? Issued { get; set; }

    public string? HomeAddress { get; set; }

    public string? MailAddress { get; set; }

    public string? TelPhone { get; set; }

    public string? MobilePhone { get; set; }

    public string? Email { get; set; }

    public string? OccupationCode { get; set; }

    public string? CustomerType { get; set; }
}
