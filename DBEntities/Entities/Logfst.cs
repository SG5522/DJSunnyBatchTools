using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Logfst
{
    public string LogSn { get; set; } = null!;

    public string? AccNo { get; set; }

    public string? Idnumber { get; set; }

    public string? CustName { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Jobcode { get; set; }

    public DateTime? Datetime { get; set; }

    public string? PassResult { get; set; }

    public string? EventUserId { get; set; }

    public string? EventName { get; set; }

    public DateTime? EventTime { get; set; }

    public string? BranchId { get; set; }

    public string? Note { get; set; }

    public string? TargetUserStaffLevel { get; set; }

    public string? EventUserStaffLevel { get; set; }
}
