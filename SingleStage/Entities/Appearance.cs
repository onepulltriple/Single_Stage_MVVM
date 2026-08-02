using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class Appearance
{
    public int Id { get; set; }

    public decimal? RoyaltyUpFront { get; set; }

    public decimal? RoyaltyAtEnd { get; set; }

    public int ArtistId { get; set; }

    public int ShowAppearanceId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ShowAppearance ShowAppearance { get; set; } = null!;
}
