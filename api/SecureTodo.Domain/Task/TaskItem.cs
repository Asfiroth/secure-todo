using SecureTodo.Domain.Base;
using SecureTodo.Domain.Task.Enums;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Domain.Task;

public sealed class TaskItem : Auditable
{
#pragma warning disable CS8618
    private TaskItem() { }
#pragma warning disable CS8618

    public TaskItem(TaskItemId id, string title, string? description, TaskPriority priority, DateOnly dueDate)
    {
        Id = id;
        Priority = priority;
        
        Title = title;
        Description = description;
        DueDate = dueDate;
    }
    
    public TaskItemId Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateOnly DueDate { get; private set; }
    
    public void MarkAsCompleted() => IsCompleted = true;
    
    public void MarkAsIncomplete() => IsCompleted = false;

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        
        if (title.Length > 120)
            throw new ArgumentException("Title cannot be longer than 120 characters.", nameof(title));
        
        Title = title;
    }

    public void UpdateDescription(string? description)
    {
        if (description is not null && description.Length > 200)
            throw new ArgumentException("Description cannot be longer than 200 characters.", nameof(description));
        
        Description = description;
    }
    
    public void UpdatePriority(TaskPriority priority) => Priority = priority;
    
    public void UpdateDueDate(DateOnly dueDate)
    {
        if (dueDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Due date cannot be in the past.", nameof(dueDate));
        
        DueDate = dueDate;
    }

    public static class Factory
    {
        public static TaskItem Create(string title, string? description, TaskPriority priority, DateOnly dueDate)
        {
            var id = TaskItemId.CreateUnique();
            var newTask = new TaskItem(id, title, description, priority, dueDate);
            
            // good place to add domain events
            
            return newTask;
        }
    }
    
}