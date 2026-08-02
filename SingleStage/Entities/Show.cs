using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class Show
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal? TicketPrice { get; set; }

    public bool SoldOut { get; set; }

    public virtual ICollection<ShowAppearance> ShowAppearances { get; set; } = new List<ShowAppearance>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
