using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Domain.Entities
{
    public class Projeto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjetoStatus Status { get; set; } = ProjetoStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Tarefa> Tarefas { get; set; } = [];
    }
}
