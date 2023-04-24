using System;
using System.Collections.Generic;

namespace ProLi.Models;

/// <summary>
/// Személyek nyilvántartására szolgáló tábla.
/// </summary>
public partial class Proliperson
{
    public int PersonId { get; set; }

    /// <summary>
    /// A nyilvántartott személy teljes neve.
    /// </summary>
    public string PersonName { get; set; } = null!;

    /// <summary>
    /// Ország
    /// </summary>
    public string PersonCountry { get; set; } = null!;

    /// <summary>
    /// 1. e-mail cím
    /// </summary>
    public string? PersonEmail { get; set; }

    /// <summary>
    /// 1. Telefonszám
    /// </summary>
    public string? PersonPhone { get; set; }

    /// <summary>
    /// Szöveges megjegyzés hozzáadása.
    /// </summary>
    public string? PersonComment { get; set; }

    public string? PersonSpecComment { get; set; }

    public sbyte? PersonStatus { get; set; }

    public virtual ICollection<Proliinvite> Proliinvites { get; } = new List<Proliinvite>();
}
