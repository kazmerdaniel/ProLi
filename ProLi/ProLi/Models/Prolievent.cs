using System;
using System.Collections.Generic;

namespace ProLi.Models;

/// <summary>
/// A rendezvények legfontosabb paramétereit tartalmazza a tábla.
/// </summary>
public partial class Prolievent
{
    public Prolievent()
    {
    }


    public int EventId { get; set; }

    /// <summary>
    /// A rendezvény megnevezése.
    /// </summary>
    public string EventName { get; set; } = null!;

    /// <summary>
    /// A rendezvény dátuma.
    /// </summary>
    public DateTime EventDate { get; set; }

    /// <summary>
    /// A rendezvény helyszínének megadása
    /// </summary>
    public string EventPlace { get; set; } = null!;

    /// <summary>
    /// A rendezvényre meghívni tervezett személyek létszáma. A maximális létszámot ajánlott megadni.
    /// </summary>
    public int EventHead { get; set; }

    public sbyte? EventStatus { get; set; }

    public virtual ICollection<Proliinvite> Proliinvites { get; } = new List<Proliinvite>();

  
}
