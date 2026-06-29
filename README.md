# TaskFlow API

API REST desenvolvida em **ASP.NET Core (.NET 8)** para gerenciamento de Projetos e Tarefas, seguindo a metodologia **Specification-Driven Development (SDD)**.

O objetivo deste projeto é demonstrar a construção de uma API orientada por especificação, onde o contrato OpenAPI é definido antes da implementação e serve como fonte da verdade durante todo o desenvolvimento.

---

# Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite ou InMemory
- OpenAPI 3.0
- Swagger
- xUnit
- WebApplicationFactory
- ProblemDetails (RFC 7807)

---

# Estrutura do Projeto

```text
TaskFlow
│
├── openapi.yaml
├── README.md
│
├── docs
│   └── decisoes.md
│
├── ai
│   ├── skills.md
│   ├── prompts.md
│   └── revisoes.md
│
├── src
│   └── TaskFlow.Api
│
└── tests
    └── TaskFlow.ContractTests
```

---

# Fluxo SDD

O desenvolvimento foi realizado seguindo as etapas propostas no desafio.

## Etapa 1 — Especificação

Antes da implementação foram produzidos:

- OpenAPI 3.0 (`openapi.yaml`)
- Documento de decisões (`docs/decisoes.md`)
- Registro do uso de IA (`ai/`)

A especificação define:

- Endpoints
- Schemas
- Request Bodies
- Responses
- Casos de erro
- Regras de negócio

---

## Etapa 2 — Implementação

A API foi implementada seguindo rigorosamente o contrato definido na especificação.

Foram utilizados:

- Controllers
- Services
- Entity Framework Core
- ProblemDetails
- ValidationProblemDetails

---

## Etapa 3 — Validação

A aderência ao contrato é garantida através de testes de integração utilizando:

- xUnit
- WebApplicationFactory

Os testes cobrem:

- Criação de recursos
- Recursos inexistentes (404)
- Violações de regras de negócio (422)
- Validação do contrato OpenAPI

---

# Regras de Negócio

A API implementa as seguintes regras:

- Não é permitido arquivar um projeto que possua tarefas com status **in_progress**.
- Não é permitido criar tarefas em projetos arquivados.
- Apenas tarefas com status **pending** podem ser excluídas.
- Ao concluir uma tarefa, o campo **completedAt** é preenchido automaticamente.
- O fluxo de status das tarefas é:

```text
pending
    ↓
in_progress
    ↓
done
```

Não é permitido retroceder o status.

---

# Como executar

## Pré-requisitos

- .NET SDK 8.0 ou superior

Verifique a instalação:

```bash
dotnet --version
```

---

## Restaurar dependências

```bash
dotnet restore
```

---

## Executar a aplicação

```bash
dotnet run --project src/TaskFlow.Api
```

---

## Swagger

Após iniciar a aplicação:

```
https://localhost:5001/swagger
```

ou

```
http://localhost:5000/swagger
```

---

# Executar os testes

```bash
dotnet test tests/TaskFlow.ContractTests
```

---

# Especificação da API

A especificação completa está disponível em:

```
openapi.yaml
```

Ela representa a fonte da verdade para toda a implementação.

---

# Organização das Pastas

```text
src/
    TaskFlow.Api/

tests/
    TaskFlow.ContractTests/

docs/
    decisoes.md

ai/
    skills.md
    prompts.md
    revisoes.md

openapi.yaml
```

---

# Autor

Desenvolvido como parte do desafio técnico **TaskFlow — Specification-Driven Development (SDD)**.