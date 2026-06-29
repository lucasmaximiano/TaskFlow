using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts.Requests;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("tarefas")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _tarefaService;

    public TarefasController(ITarefaService tarefaService)
    {
        _tarefaService = tarefaService;
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarTarefaRequest request)
    {
        var result = await _tarefaService.AtualizarAsync(id, request);

        if (!result.Success)
            return StatusCode(result.Error!.Status!.Value, result.Error);

        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir([FromRoute] Guid id)
    {
        var result = await _tarefaService.ExcluirAsync(id);

        if (!result.Success)
            return StatusCode(result.Error!.Status!.Value, result.Error);

        return NoContent();
    }
}