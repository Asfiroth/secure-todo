namespace SecureTodo.Domain.Task.ValueObjects;

public sealed record TaskItemId
{
    public Guid Value { get; }

    private TaskItemId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TaskId cannot be empty.", nameof(value));
		
        Value = value;
    }

    public static TaskItemId FromGuid(Guid id) => new(id);
    public static TaskItemId CreateUnique() => new(Guid.NewGuid());
	
    public override string ToString() => Value.ToString();
}