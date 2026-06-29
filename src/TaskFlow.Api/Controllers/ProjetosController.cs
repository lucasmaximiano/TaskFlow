using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts.Requests;
using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("projetos")]
public class ProjetosController : ControllerBase
{
    private readonly IProjetoService _projetoService;
    private readonly ITarefaService _tarefaService;

    public ProjetosController(
        IProjetoService projetoService,
        ITarefaService tarefaService)
    {
        _projetoService = projetoService;
        _tarefaService = tarefaService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarProjetoRequest request)
    {
        var result = await _projetoService.CriarAsync(request);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = result.Data!.Id },
            result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] ProjetoStatus? status)
    {
        var result = await _projetoService.ListarAsync(status);

        return Ok(result.Data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId([FromRoute] Guid id)
    {
        var result = await _projetoService.BuscarPorIdAsync(id);

        if (!result.Success)
            return StatusCode(result.Error!.Status!.Value, result.Error);

        return Ok(result.Data);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarProjetoRequest request)
    {
        var result = await _projetoService.AtualizarAsync(id, request);

        if (!result.Success)
            return StatusCode(result.Error!.Status!.Value, result.Error);

        return Ok(result.Data);
    }

    [HttpPost("{id:guid}/tarefas")]
    public async Task<IActionResult> CriarTarefa(
        [FromRoute] Guid id,
        [FromBody] CriarTarefaRequest request)
    {
        var result = await _tarefaService.CriarAsync(id, request);

        if (!result.Success)
            return StatusCode(result.Error!.Status!.Value, result.Error);

        return CreatedAtAction(
            nameof(ListarTarefas),
            new { id },
            result.Data);
    }

    [HttpGet("{id:guid}/tarefas")]
    public async Task<IActionResult> ListarTarefas(
        [FromRoute] Guid id,
        [FromQuery] TarefaStatus? status,
        [FromQuery] Prioridade? priority)
    {
        var result = await _tarefaService.ListarPorProjetoAsync(id, status, priority);

        if (!result.Success)
            return StatusCode(result.Error!.Status!.Value, result.Error);

        return Ok(result.Data);
    }
}