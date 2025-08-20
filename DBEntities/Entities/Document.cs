using System;
using System.Collections.Generic;

namespace DBEntities.Entities;

public partial class Document
{
    public string Accno { get; set; } = null!;

    public string SetOrder { get; set; } = null!;

    public string DocId { get; set; } = null!;

    public string? Imagefpath { get; set; }

    public string? Imagebpath { get; set; }
}
