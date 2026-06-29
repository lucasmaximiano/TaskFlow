using System.ComponentModel.DataAnnotations;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Contracts.Requests
{
    public class CriarTarefaRequest
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required(ErrorMessage = "A prioridade é obrigatória.")]
        public Prioridade Priority { get; set; }
    }
}
