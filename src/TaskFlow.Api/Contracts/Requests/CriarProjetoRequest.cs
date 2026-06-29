using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Contracts.Requests
{
    public class CriarProjetoRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
