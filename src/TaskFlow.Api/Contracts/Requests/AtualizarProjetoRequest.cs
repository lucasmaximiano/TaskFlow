using System.ComponentModel.DataAnnotations;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Contracts.Requests
{
    public class AtualizarProjetoRequest
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ProjetoStatus? Status { get; set; }
    }
}
