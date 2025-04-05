namespace DataLayer.Entities;

public sealed class Poll : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Sammary { get; set; } = null!;
    public bool IsPublished { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public ICollection<Question> Questions { get; set; } = [];

    public ICollection<Vote> Votes { get; set; } = [];


}
