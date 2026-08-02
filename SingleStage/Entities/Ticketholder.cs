using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class Ticketholder
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string Email { get; set; } = null!;

    public bool Discount { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
