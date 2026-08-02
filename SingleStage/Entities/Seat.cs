using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class Seat
{
    public int Id { get; set; }

    public string Row { get; set; } = null!;

    public int Number { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
