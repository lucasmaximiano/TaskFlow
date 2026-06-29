using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Common;
using TaskFlow.Api.Contracts.Requests;
using TaskFlow.Api.Contracts.Responses;
using TaskFlow.Api.Domain.Entities;
using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Infrastructure.Context;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services;

public class ProjetoService : IProjetoService
{
    private readonly TaskFlowDbContext _context;

    public ProjetoService(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult<ProjetoResponse>> CriarAsync(CriarProjetoRequest request)
    {
        var projeto = new Projeto
        {
            Name = request.Name,
            Description = request.Description,
            Status = ProjetoStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projetos.Add(projeto);
        await _context.SaveChangesAsync();

        return ServiceResult<ProjetoResponse>.Ok(Map(projeto));
    }

    public async Task<ServiceResult<List<ProjetoResponse>>> ListarAsync(ProjetoStatus? status)
    {
        var query = _context.Projetos.AsQueryable();

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        var projetos = await query
            .Select(x => Map(x))
            .ToListAsync();

        return ServiceResult<List<ProjetoResponse>>.Ok(projetos);
    }

    public async Task<ServiceResult<ProjetoResponse>> BuscarPorIdAsync(Guid id)
    {
        var projeto = await _context.Projetos.FindAsync(id);

        if (projeto is null)
        {
            return ServiceResult<ProjetoResponse>.Fail(
                StatusCodes.Status404NotFound,
                "Projeto não encontrado",
                "Não foi encontrado um projeto com o ID informado.");
        }

        return ServiceResult<ProjetoResponse>.Ok(Map(projeto));
    }

    public async Task<ServiceResult<ProjetoResponse>> AtualizarAsync(
        Guid id,
        AtualizarProjetoRequest request)
    {
        var projeto = await _context.Projetos
            .Include(x => x.Tarefas)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (projeto is null)
        {
            return ServiceResult<ProjetoResponse>.Fail(
                StatusCodes.Status404NotFound,
                "Projeto não encontrado",
                "Não foi encontrado um projeto com o ID informado.");
        }

        if (request.Status == ProjetoStatus.Archived)
        {
            var possuiTarefaEmAndamento = projeto.Tarefas
                .Any(x => x.Status == TarefaStatus.InProgress);

            if (possuiTarefaEmAndamento)
            {
                return ServiceResult<ProjetoResponse>.Fail(
                    StatusCodes.Status422UnprocessableEntity,
                    "Regra de negócio violada",
                    "Não é possível arquivar um projeto com tarefas em andamento.");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
            projeto.Name = request.Name;

        if (request.Description is not null)
            projeto.Description = request.Description;

        if (request.Status.HasValue)
            projeto.Status = request.Status.Value;

        await _context.SaveChangesAsync();

        return ServiceResult<ProjetoResponse>.Ok(Map(projeto));
    }

    private static ProjetoResponse Map(Projeto projeto)
    {
        return new ProjetoResponse
        {
            Id = projeto.Id,
            Name = projeto.Name,
            Description = projeto.Description,
            Status = projeto.Status switch
            {
                ProjetoStatus.Active => "active",
                ProjetoStatus.Archived => "archived",
                _ => projeto.Status.ToString()
            },
            CreatedAt = projeto.CreatedAt
        };
    }
}