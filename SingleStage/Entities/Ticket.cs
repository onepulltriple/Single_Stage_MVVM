using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class Ticket
{
    public int Id { get; set; }

    public int TicketholderId { get; set; }

    public int SeatId { get; set; }

    public int ShowId { get; set; }

    public virtual Seat Seat { get; set; } = null!;

    public virtual Show Show { get; set; } = null!;

    public virtual Ticketholder Ticketholder { get; set; } = null!;
}
