namespace CareLink360.Domain.Entities;

public class Contract
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid CaregiverId { get; set; }

    public Guid JobRequestId { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}