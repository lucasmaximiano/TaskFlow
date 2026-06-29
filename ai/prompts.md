# Prompts Utilizados

Este arquivo registra os principais prompts utilizados durante o desenvolvimento do desafio.

## Prompt 1 — Entendimento do desafio

**Prompt utilizado:**

"Revise este desafio técnico de .NET com Specification-Driven Development e me ajude a identificar quais entregáveis são obrigatórios."

**Resultado produzido:**

A IA ajudou a identificar os principais artefatos exigidos:

- openapi.yaml
- docs/decisoes.md
- ai/skills.md
- ai/prompts.md
- ai/revisoes.md
- implementação em ASP.NET Core
- testes com xUnit e WebApplicationFactory

## Prompt 2 — Criação do contrato OpenAPI

**Prompt utilizado:**

"Gere um openapi.yaml completo para uma API de projetos e tarefas em ASP.NET Core seguindo estas regras de negócio."

**Resultado produzido:**

A IA gerou uma primeira versão do arquivo OpenAPI contendo:

- paths
- métodos HTTP
- schemas
- request bodies
- responses
- status codes
- ProblemDetails
- ValidationProblemDetails

## Prompt 3 — Revisão da especificação

**Prompt utilizado:**

"Revise se o openapi.yaml cobre todos os endpoints, filtros, erros e regras de negócio do desafio."

**Resultado produzido:**

A IA apontou pontos importantes de revisão:

- garantir responses 422 nas regras de negócio
- manter completedAt como readOnly
- não expor completedAt nos requests
- incluir filtros por status e prioridade
- padronizar enums

## Prompt 4 — Documentação de decisões

**Prompt utilizado:**

"Crie um docs/decisoes.md explicando as decisões de design da API, tratamento de erros e estrutura de dados."

**Resultado produzido:**

A IA gerou uma estrutura de documentação explicando:

- abordagem SDD
- escolha de entidades
- relacionamento entre projeto e tarefa
- tratamento de erros
- decisões sobre persistência
- estratégia de testes

## Prompt 5 — README

**Prompt utilizado:**

"Crie um README.md profissional para o desafio técnico TaskFlow, incluindo pré-requisitos, como rodar a aplicação e como executar os testes."

**Resultado produzido:**

A IA gerou um README com:

- descrição do projeto
- tecnologias utilizadas
- estrutura de pastas
- instruções de execução
- instruções de testes
- resumo das regras de negócio