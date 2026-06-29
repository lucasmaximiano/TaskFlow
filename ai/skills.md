# Uso de IA — Skills Delegadas

Durante o desenvolvimento do desafio TaskFlow, a IA foi utilizada como ferramenta de apoio nas seguintes áreas:

## 1. Modelagem da especificação OpenAPI

A IA foi utilizada para auxiliar na criação inicial do contrato OpenAPI 3.0, incluindo:

- definição dos endpoints
- modelagem dos schemas
- padronização dos responses
- inclusão dos status codes exigidos
- representação dos erros usando ProblemDetails e ValidationProblemDetails

## 2. Revisão das regras de negócio

A IA foi utilizada para revisar se as regras de negócio estavam refletidas na especificação, especialmente:

- impedimento de arquivamento de projeto com tarefas em andamento
- bloqueio de criação de tarefas em projeto arquivado
- exclusão restrita a tarefas pendentes
- preenchimento automático de completedAt
- controle de transição de status da tarefa

## 3. Organização da solução

A IA foi utilizada para sugerir uma estrutura inicial de projeto em .NET, separando responsabilidades entre:

- Controllers
- Services/UseCases
- Entidades
- Contracts
- Infrastructure
- Tests

## 4. Documentação

A IA apoiou a criação de documentos auxiliares, como:

- README.md
- docs/decisoes.md
- registros de prompts
- registros de revisão crítica

## 5. Testes de contrato

A IA será utilizada como apoio para estruturar testes com:

- xUnit
- WebApplicationFactory
- System.Net.Http.Json

A validação final dos testes e da aderência ao contrato será feita manualmente.