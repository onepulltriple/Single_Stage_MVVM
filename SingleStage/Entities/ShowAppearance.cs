using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class ShowAppearance
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int ShowId { get; set; }

    public virtual ICollection<Appearance> Appearances { get; set; } = new List<Appearance>();

    public virtual Show Show { get; set; } = null!;
}
