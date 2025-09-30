﻿namespace SurveyBasket.Entities;

public class AuditableEntity
{
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public ApplicationUser CreatedBy { get; set; } = default!;

    public string? UpdatedById { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public ApplicationUser? UpdatedBy { get; set; } = default!;
}
