using System;
using System.Collections.Generic;

namespace SingleStage.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
