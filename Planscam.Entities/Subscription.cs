using System.ComponentModel.DataAnnotations.Schema;

namespace Planscam.Entities;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    [Column(TypeName = "Money")]
    public decimal Price { get; set; }
    public SubscriptionDurations Duration { get; set; }
    
    public enum SubscriptionDurations
    {
        Month,
        ThreeMonths,
        Year
    }
}
