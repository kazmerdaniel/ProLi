using System;
using System.Collections.Generic;

namespace ProLi.Models;

public partial class Proliinvite
{
    public int InviteId { get; set; }

    public int EventId { get; set; }

    public int PersonId { get; set; }

    public virtual Prolievent Event { get; set; } = null!;

    public virtual Proliperson Person { get; set; } = null!;
}
