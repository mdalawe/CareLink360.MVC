namespace CareLink360.Domain.Entities;

public class NinVerification
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Nin { get; set; } = string.Empty;

    public bool IsVerified { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime SubmittedAt { get; set; }

    public DateTime? VerifiedAt { get; set; }
}