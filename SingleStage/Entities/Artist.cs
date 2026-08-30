using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class Artist
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ArtistPerformance> ArtistPerformances { get; set; } = new List<ArtistPerformance>();
}
