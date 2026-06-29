using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Domain.Entities;

namespace TaskFlow.Api.Infrastructure.Context;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Projeto> Projetos => Set<Projeto>();

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Projeto>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description);

            entity.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasMany(x => x.Tarefas)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId);
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description);

            entity.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.Property(x => x.CompletedAt);
        });
    }
}