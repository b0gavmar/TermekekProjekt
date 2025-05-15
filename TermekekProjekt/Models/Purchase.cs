using System;
using System.Collections.Generic;

namespace TermekekProjekt.Models;

public partial class Purchase
{
    public int? CustomerId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public DateOnly? Date { get; set; }
}
