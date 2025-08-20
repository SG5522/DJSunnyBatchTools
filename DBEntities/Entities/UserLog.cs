using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class UserLog
{
    public long Id { get; set; }

    public string? JobCode { get; set; }

    public DateTime? ExecuteTime { get; set; }

    public string? Note { get; set; }

    public string? TargetUserId { get; set; }

    public string? TargetBranchId { get; set; }

    public string? TargetUserName { get; set; }

    public string? TargetStaffLevel { get; set; }

    public string? TargetStatus { get; set; }

    public string? TargetPermission { get; set; }

    public DateTime? TargetExpiryDate { get; set; }

    public string? TargetRemark { get; set; }

    public string? ExecutiveUserId { get; set; }

    public string? ExecutiveBranchId { get; set; }

    public string? ExecutiveUserName { get; set; }

    public string? ExecutiveStaffLevel { get; set; }

    public string? ExecutiveStatus { get; set; }

    public string? ExecutivePermission { get; set; }

    public DateTime? ExecutiveExpiryDate { get; set; }

    public string? ExecutiveRemark { get; set; }
}
