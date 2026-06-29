using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Common;
using TaskFlow.Api.Contracts.Requests;
using TaskFlow.Api.Contracts.Responses;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Services.Interfaces
{
    public interface ITarefaService
    {
        Task<ServiceResult<TarefaResponse>> CriarAsync(Guid projetoId, CriarTarefaRequest request); 
        Task<ServiceResult<List<TarefaResponse>>> ListarPorProjetoAsync(Guid projetoId, TarefaStatus? status, Prioridade? priority); 
        Task<ServiceResult<TarefaResponse>> AtualizarAsync(Guid id, AtualizarTarefaRequest request); 
        Task<ServiceResult<bool>> ExcluirAsync(Guid id);
    }
}
