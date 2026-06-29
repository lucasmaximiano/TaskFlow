using TaskFlow.Api.Common;
using TaskFlow.Api.Contracts.Requests;
using TaskFlow.Api.Contracts.Responses;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Services.Interfaces
{
    public interface IProjetoService
    {
        Task<ServiceResult<ProjetoResponse>> CriarAsync(CriarProjetoRequest request); 
        Task<ServiceResult<List<ProjetoResponse>>> ListarAsync(ProjetoStatus? status); 
        Task<ServiceResult<ProjetoResponse>> BuscarPorIdAsync(Guid id); 
        Task<ServiceResult<ProjetoResponse>> AtualizarAsync(Guid id, AtualizarProjetoRequest request);
    }
}
