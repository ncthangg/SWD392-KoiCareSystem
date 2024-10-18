﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace KoiCareSystem.Data.Models;

public partial class Pond
{
    public int PondId { get; set; }

    public string PondName { get; set; }

    public string ImageUrl { get; set; }

    public decimal? Size { get; set; }

    public decimal? Depth { get; set; }

    public decimal? Volume { get; set; }

    public int? DrainCount { get; set; }

    public decimal? PumpCapacity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? SkimmerCount { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<KoiFish> KoiFishes { get; set; } = new List<KoiFish>();

    public virtual User User { get; set; }

    public virtual ICollection<WaterParameter> WaterParameters { get; set; } = new List<WaterParameter>();
}