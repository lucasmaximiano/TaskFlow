using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Domain.Entities
{
    public class Tarefa
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TarefaStatus Status { get; set; } = TarefaStatus.Pending;
        public Prioridade Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public Guid ProjectId { get; set; }
        public Projeto Project { get; set; } = null!;
    }
}
