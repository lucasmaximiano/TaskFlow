using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Common;
using TaskFlow.Api.Contracts.Requests;
using TaskFlow.Api.Contracts.Responses;
using TaskFlow.Api.Domain.Entities;
using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Infrastructure.Context;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services;

public class TarefaService : ITarefaService
{
    private readonly TaskFlowDbContext _context;

    public TarefaService(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult<TarefaResponse>> CriarAsync(
        Guid projetoId,
        CriarTarefaRequest request)
    {
        var projeto = await _context.Projetos.FirstOrDefaultAsync(x => x.Id == projetoId);

        if (projeto is null)
        {
            return ServiceResult<TarefaResponse>.Fail(
                StatusCodes.Status404NotFound,
                "Projeto não encontrado",
                "Não foi encontrado um projeto com o ID informado.");
        }

        if (projeto.Status == ProjetoStatus.Archived)
        {
            return ServiceResult<TarefaResponse>.Fail(
                StatusCodes.Status422UnprocessableEntity,
                "Regra de negócio violada",
                "Não é permitido criar tarefas em um projeto arquivado.");
        }

        var tarefa = new Tarefa
        {
            Title = request.Title,
            Description = request.Description,
            Status = TarefaStatus.Pending,
            Priority = request.Priority,
            CreatedAt = DateTime.UtcNow,
            ProjectId = projetoId
        };

        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();

        return ServiceResult<TarefaResponse>.Ok(Map(tarefa));
    }

    public async Task<ServiceResult<List<TarefaResponse>>> ListarPorProjetoAsync(
        Guid projetoId,
        TarefaStatus? status,
        Prioridade? priority)
    {
        var projetoExiste = await _context.Projetos.AnyAsync(x => x.Id == projetoId);

        if (!projetoExiste)
        {
            return ServiceResult<List<TarefaResponse>>.Fail(
                StatusCodes.Status404NotFound,
                "Projeto não encontrado",
                "Não foi encontrado um projeto com o ID informado.");
        }

        var query = _context.Tarefas.Where(x => x.ProjectId == projetoId);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(x => x.Priority == priority.Value);

        var tarefas = await query.Select(x => Map(x)).ToListAsync();

        return ServiceResult<List<TarefaResponse>>.Ok(tarefas);
    }

    public async Task<ServiceResult<TarefaResponse>> AtualizarAsync(
        Guid id,
        AtualizarTarefaRequest request)
    {
        var tarefa = await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id);

        if (tarefa is null)
        {
            return ServiceResult<TarefaResponse>.Fail(
                StatusCodes.Status404NotFound,
                "Tarefa não encontrada",
                "Não foi encontrada uma tarefa com o ID informado.");
        }

        if (request.Status.HasValue)
        {
            if (!EhTransicaoValida(tarefa.Status, request.Status.Value))
            {
                return ServiceResult<TarefaResponse>.Fail(
                    StatusCodes.Status422UnprocessableEntity,
                    "Regra de negócio violada",
                    "Transição de status inválida. O fluxo permitido é pending -> in_progress -> done, sem retrocesso.");
            }

            tarefa.Status = request.Status.Value;

            if (tarefa.Status == TarefaStatus.Done && tarefa.CompletedAt is null)
                tarefa.CompletedAt = DateTime.UtcNow;
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
            tarefa.Title = request.Title;

        if (request.Description is not null)
            tarefa.Description = request.Description;

        if (request.Priority.HasValue)
            tarefa.Priority = request.Priority.Value;

        await _context.SaveChangesAsync();

        return ServiceResult<TarefaResponse>.Ok(Map(tarefa));
    }

    public async Task<ServiceResult<bool>> ExcluirAsync(Guid id)
    {
        var tarefa = await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id);

        if (tarefa is null)
        {
            return ServiceResult<bool>.Fail(
                StatusCodes.Status404NotFound,
                "Tarefa não encontrada",
                "Não foi encontrada uma tarefa com o ID informado.");
        }

        if (tarefa.Status != TarefaStatus.Pending)
        {
            return ServiceResult<bool>.Fail(
                StatusCodes.Status422UnprocessableEntity,
                "Regra de negócio violada",
                "Apenas tarefas com status pending podem ser excluídas.");
        }

        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();

        return ServiceResult<bool>.Ok(true);
    }

    private static bool EhTransicaoValida(TarefaStatus atual, TarefaStatus novo)
    {
        if (atual == novo)
            return true;

        return atual switch
        {
            TarefaStatus.Pending => novo == TarefaStatus.InProgress,
            TarefaStatus.InProgress => novo == TarefaStatus.Done,
            TarefaStatus.Done => false,
            _ => false
        };
    }

    private static TarefaResponse Map(Tarefa tarefa)
    {
        return new TarefaResponse
        {
            Id = tarefa.Id,
            Title = tarefa.Title,
            Description = tarefa.Description,
            Status = tarefa.Status switch
            {
                TarefaStatus.Pending => "pending",
                TarefaStatus.InProgress => "in_progress",
                TarefaStatus.Done => "done",
                _ => tarefa.Status.ToString()
            },
            Priority = tarefa.Priority switch
            {
                Prioridade.Low => "low",
                Prioridade.Medium => "medium",
                Prioridade.High => "high",
                _ => tarefa.Priority.ToString()
            },
            CreatedAt = tarefa.CreatedAt,
            CompletedAt = tarefa.CompletedAt,
            ProjectId = tarefa.ProjectId
        };
    }
}
