# Revisões Críticas das Sugestões da IA

Este arquivo documenta o que foi revisado, corrigido ou rejeitado das sugestões produzidas pela IA.

## 1. Ajuste nos endpoints

A IA inicialmente sugeriu incluir o endpoint `DELETE /projetos/{id}`.

Esse endpoint foi removido porque não faz parte dos endpoints mínimos obrigatórios do desafio.

Endpoints finais mantidos:

- POST /projetos
- GET /projetos
- GET /projetos/{id}
- PATCH /projetos/{id}
- POST /projetos/{id}/tarefas
- GET /projetos/{id}/tarefas
- PATCH /tarefas/{id}
- DELETE /tarefas/{id}

## 2. Ajuste nos status das tarefas

A IA inicialmente sugeriu nomes de status como `todo`, `inProgress` e `done`.

Esses valores foram corrigidos para seguir exatamente o enunciado:

- pending
- in_progress
- done

## 3. Ajuste nos status dos projetos

A IA manteve os status de projeto conforme o desafio:

- active
- archived

Foi validado que o status padrão deve ser `active`.

## 4. Campo completedAt

Foi revisado que o campo `completedAt` não deve aparecer em nenhum request.

Ele deve ser apenas retornado no response e marcado como controlado pela aplicação.

No OpenAPI, esse campo foi mantido apenas em `TarefaResponse` com `readOnly: true`.

## 5. Regras de negócio com status 422

Foi revisado que as regras de negócio devem retornar `422 Unprocessable Entity`, e não `400 Bad Request`.

As seguintes situações foram mapeadas para 422:

- arquivar projeto com tarefas em andamento
- criar tarefa em projeto arquivado
- excluir tarefa que não esteja pendente
- tentar retroceder status de tarefa
- tentar pular ou violar a transição permitida de status

## 6. Validações com status 400

Foi definido que erros de formato, campos obrigatórios e tamanho máximo devem retornar `400` com `ValidationProblemDetails`.

Exemplos:

- name obrigatório
- name maior que 100 caracteres
- title obrigatório
- title maior que 200 caracteres
- priority inválida
- status inválido

## 7. Estrutura do projeto

A IA sugeriu uma estrutura simples inicialmente.

A estrutura foi ajustada para separar melhor as responsabilidades:

- `src/TaskFlow.Api`
- `tests/TaskFlow.ContractTests`
- `docs`
- `ai`
- `openapi.yaml`

## 8. Persistência

A IA sugeriu SQLite ou InMemory.

Foi escolhido inicialmente EF Core InMemory por simplicidade no desafio e facilidade nos testes de contrato.

A escolha ainda pode ser alterada para SQLite sem impacto no contrato da API.

## 9. Conclusão da revisão

As sugestões da IA foram utilizadas como ponto de partida, mas revisadas manualmente para garantir aderência ao enunciado.

A especificação OpenAPI foi tratada como fonte da verdade para a implementação.