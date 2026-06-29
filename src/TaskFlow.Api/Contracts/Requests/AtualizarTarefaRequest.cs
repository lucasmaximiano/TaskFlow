using System.ComponentModel.DataAnnotations;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Contracts.Requests
{
    public class AtualizarTarefaRequest
    {
        [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TarefaStatus? Status { get; set; }
        public Prioridade? Priority { get; set; }
    }
}
