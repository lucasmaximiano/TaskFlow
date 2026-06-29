using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TaskFlow.ContractTests;

public class TaskFlowApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TaskFlowApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostProjetos_DeveCriarProjetoERetornar201()
    {
        var response = await CriarProjetoResponseAsync();

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<ProjetoResponseTest>();

        body.Should().NotBeNull();
        body!.id.Should().NotBeEmpty();
        body.name.Should().NotBeNullOrWhiteSpace();
        body.status.Should().Be("active");
    }

    [Fact]
    public async Task GetProjetoPorId_QuandoNaoExiste_DeveRetornar404()
    {
        var response = await _client.GetAsync($"/projetos/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProjetos_ComFiltroStatus_DeveRetornar200()
    {
        await CriarProjetoAsync();

        var response = await _client.GetAsync("/projetos?status=active");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<List<ProjetoResponseTest>>();

        body.Should().NotBeNull();
        body!.Should().OnlyContain(x => x.status == "active");
    }

    [Fact]
    public async Task PatchProjeto_ArquivarComTarefaEmAndamento_DeveRetornar422()
    {
        var projeto = await CriarProjetoAsync();
        var tarefa = await CriarTarefaAsync(projeto.id);

        await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "inProgress"
        });

        var response = await _client.PatchAsJsonAsync($"/projetos/{projeto.id}", new
        {
            status = "archived"
        });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task PostTarefas_DeveCriarTarefaERetornar201()
    {
        var projeto = await CriarProjetoAsync();

        var response = await CriarTarefaResponseAsync(projeto.id);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<TarefaResponseTest>();

        body.Should().NotBeNull();
        body!.id.Should().NotBeEmpty();
        body.status.Should().Be("pending");
        body.priority.Should().Be("high");
        body.projectId.Should().Be(projeto.id);
    }

    [Fact]
    public async Task PostTarefa_ProjetoInexistente_DeveRetornar404()
    {
        var response = await CriarTarefaResponseAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostTarefa_ProjetoArquivado_DeveRetornar422()
    {
        var projeto = await CriarProjetoAsync();

        await _client.PatchAsJsonAsync($"/projetos/{projeto.id}", new
        {
            status = "archived"
        });

        var response = await CriarTarefaResponseAsync(projeto.id);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task GetTarefas_ComFiltrosStatusEPriority_DeveRetornar200()
    {
        var projeto = await CriarProjetoAsync();
        await CriarTarefaAsync(projeto.id);

        var response = await _client.GetAsync($"/projetos/{projeto.id}/tarefas?status=pending&priority=high");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<List<TarefaResponseTest>>();

        body.Should().NotBeNull();
        body!.Should().OnlyContain(x => x.status == "pending" && x.priority == "high");
    }

    [Fact]
    public async Task GetTarefas_ProjetoInexistente_DeveRetornar404()
    {
        var response = await _client.GetAsync($"/projetos/{Guid.NewGuid()}/tarefas");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PatchTarefa_PendingParaInProgress_DeveRetornar200()
    {
        var tarefa = await CriarTarefaCompletaAsync();

        var response = await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "inProgress"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<TarefaResponseTest>();

        body!.status.Should().Be("in_progress");
        body.completedAt.Should().BeNull();
    }

    [Fact]
    public async Task PatchTarefa_InProgressParaDone_DevePreencherCompletedAt()
    {
        var tarefa = await CriarTarefaCompletaAsync();

        await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "inProgress"
        });

        var response = await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "done"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<TarefaResponseTest>();

        body!.status.Should().Be("done");
        body.completedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task PatchTarefa_DoneParaPending_DeveRetornar422()
    {
        var tarefa = await CriarTarefaDoneAsync();

        var response = await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "pending"
        });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task PatchTarefa_DoneParaInProgress_DeveRetornar422()
    {
        var tarefa = await CriarTarefaDoneAsync();

        var response = await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "inProgress"
        });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task PatchTarefa_Inexistente_DeveRetornar404()
    {
        var response = await AtualizarTarefaAsync(Guid.NewGuid(), new
        {
            title = "Nova tarefa"
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTarefa_Pending_DeveRetornar204()
    {
        var tarefa = await CriarTarefaCompletaAsync();

        var response = await _client.DeleteAsync($"/tarefas/{tarefa.id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTarefa_InProgress_DeveRetornar422()
    {
        var tarefa = await CriarTarefaCompletaAsync();

        await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "inProgress"
        });

        var response = await _client.DeleteAsync($"/tarefas/{tarefa.id}");

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task DeleteTarefa_Done_DeveRetornar422()
    {
        var tarefa = await CriarTarefaDoneAsync();

        var response = await _client.DeleteAsync($"/tarefas/{tarefa.id}");

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task DeleteTarefa_Inexistente_DeveRetornar404()
    {
        var response = await _client.DeleteAsync($"/tarefas/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<HttpResponseMessage> CriarProjetoResponseAsync()
    {
        return await _client.PostAsJsonAsync("/projetos", new
        {
            name = $"Projeto {Guid.NewGuid()}",
            description = "Projeto criado pelo teste"
        });
    }

    private async Task<ProjetoResponseTest> CriarProjetoAsync()
    {
        var response = await CriarProjetoResponseAsync();

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var projeto = await response.Content.ReadFromJsonAsync<ProjetoResponseTest>();

        projeto.Should().NotBeNull();

        return projeto!;
    }

    private async Task<HttpResponseMessage> CriarTarefaResponseAsync(Guid projetoId)
    {
        return await _client.PostAsJsonAsync($"/projetos/{projetoId}/tarefas", new
        {
            title = $"Tarefa {Guid.NewGuid()}",
            description = "Tarefa criada pelo teste",
            priority = "high"
        });
    }

    private async Task<TarefaResponseTest> CriarTarefaAsync(Guid projetoId)
    {
        var response = await CriarTarefaResponseAsync(projetoId);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var tarefa = await response.Content.ReadFromJsonAsync<TarefaResponseTest>();

        tarefa.Should().NotBeNull();

        return tarefa!;
    }

    private async Task<TarefaResponseTest> CriarTarefaCompletaAsync()
    {
        var projeto = await CriarProjetoAsync();

        return await CriarTarefaAsync(projeto.id);
    }

    private async Task<TarefaResponseTest> CriarTarefaDoneAsync()
    {
        var tarefa = await CriarTarefaCompletaAsync();

        await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "inProgress"
        });

        var response = await AtualizarTarefaAsync(tarefa.id, new
        {
            status = "done"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tarefaDone = await response.Content.ReadFromJsonAsync<TarefaResponseTest>();

        tarefaDone.Should().NotBeNull();

        return tarefaDone!;
    }

    private async Task<HttpResponseMessage> AtualizarTarefaAsync(Guid tarefaId, object request)
    {
        return await _client.PatchAsJsonAsync($"/tarefas/{tarefaId}", request);
    }

    private sealed class ProjetoResponseTest
    {
        public Guid id { get; set; }

        public string name { get; set; } = string.Empty;

        public string? description { get; set; }

        public string status { get; set; } = string.Empty;

        public DateTime createdAt { get; set; }
    }

    private sealed class TarefaResponseTest
    {
        public Guid id { get; set; }

        public string title { get; set; } = string.Empty;

        public string? description { get; set; }

        public string status { get; set; } = string.Empty;

        public string priority { get; set; } = string.Empty;

        public DateTime createdAt { get; set; }

        public DateTime? completedAt { get; set; }

        public Guid projectId { get; set; }
    }
}
