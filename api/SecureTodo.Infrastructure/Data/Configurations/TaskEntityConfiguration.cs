using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureTodo.Domain.Task;
using SecureTodo.Domain.Task.ValueObjects;

namespace SecureTodo.Infrastructure.Data.Configurations;

public sealed class TaskEntityConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TaskItemId.FromGuid(value));
        
        builder
            .Property(t => t.Title)
            .HasMaxLength(120)
            .IsRequired();
        
        builder
            .Property(t => t.Description)
            .HasMaxLength(200)
            .IsRequired(false);
        
        builder
            .Property(t => t.Priority)
            .IsRequired();
        
        builder
            .Property(t => t.DueDate)
            .IsRequired();
        
        // Auditing properties are handled as shadow properties to avoid domain pollution
        builder
            .Property<Guid>("CreatedBy")
            .ValueGeneratedNever()
            .IsRequired();
        
        builder
            .Property<DateTimeOffset>("CreatedAt")
            .HasColumnType("datetimeoffset")
            .HasDefaultValueSql("SYSDATETIMEOFFSET()")
            .ValueGeneratedOnAdd();
        
        builder
            .Property<Guid?>("UpdatedBy")
            .ValueGeneratedNever();
        
        builder
            .Property<DateTimeOffset?>("UpdatedAt")
            .HasColumnType("datetimeoffset");
        
        builder
            .Property<bool>("IsDeleted")
            .HasDefaultValue(false);
        
        builder
            .Property<Guid?>("DeletedBy")
            .ValueGeneratedNever();
		
        builder
            .Property<DateTimeOffset?>("DeletedAt")
            .HasColumnType("datetimeoffset");
		
        // Assuring that only non-deleted entities are retrieved
        builder.HasQueryFilter(p => !EF.Property<bool>(p, "IsDeleted"));
    }
}