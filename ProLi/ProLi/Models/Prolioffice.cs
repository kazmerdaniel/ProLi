using System;
using System.Collections.Generic;

namespace ProLi.Models;

/// <summary>
/// A megadott személy által betöltött jogviszony adatait tartalmazza a tábla. 
/// A személy egyedi azonosítója alapján rendeljük a személyhez a szervezetetet, amelyet  legalább egy legfeljebb három szinten azonosítunk. Tartalmazza a betöltött tisztséget. A jogviszony kezdetét (kötelezően) és végét (opcionális). 
/// Amennyiben a jogviszony vége nincs megadva, akkor az adott szervezetnél a megadott pozícióban aktív státuszban áll.
/// Egy időben egy személy nulla, egy vagy több pozíciót is betölthet.
/// </summary>
public partial class Prolioffice
{
    public int OfficeId { get; set; }

    public int PersonId { get; set; }

    /// <summary>
    /// A betöltött pozíció, munkakör megnevezése. 
    /// </summary>
    public string OfficePost { get; set; } = null!;

    /// <summary>
    /// Az adott szervezetnél, az adott jogviszony kezdetének dátuma.
    /// </summary>
    public DateTime OfficeStart { get; set; }

    /// <summary>
    /// Az adott szervezetnél, az adott jogviszony befejezésének dátuma. Üres érték esetén a jogviszony aktívnak tekintendő.
    /// </summary>
    public DateTime? OfficeEnd { get; set; }

    /// <summary>
    /// A szervezet 1. szintjének megnevezése. Kötelező megadni legalább egy szinten a szervezeti struktúrát, ahol az adott személy tevékenykedik a megadott pozícióban.
    /// </summary>
    public string OfficeName1 { get; set; } = null!;

    public string? OfficeName2 { get; set; }

    public string? OfficeName3 { get; set; }

    public string? OfficeAddress { get; set; }

    public string? OfficeEmail { get; set; }

    public string? OfficePhone { get; set; }
}
