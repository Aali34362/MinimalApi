namespace Exotic.WebHook.Domain.Models;

public class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the entity

    // Creation details
    public string CreatedBy { get; set; } = "Admin"; // User who created the record
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Timestamp of creation (use UTC for consistency)

    // Last modification details
    public string LastModifiedBy { get; set; } = "Admin"; // User who last modified the record
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow; // Timestamp of last modification (use UTC)

    // Soft delete indicators
    public bool IsActive { get; set; } = true; // Indicates if the record is active
    public bool IsDeleted { get; set; } = false; // Indicates if the record is soft-deleted
}