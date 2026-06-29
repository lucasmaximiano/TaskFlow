# Decisões de Design — TaskFlow API

## 1. Abordagem SDD

O projeto segue Specification-Driven Development. Antes da implementação, foi definido o contrato da API usando OpenAPI 3.0.3.

A especificação é a fonte da verdade para:

- endpoints
- request bodies
- responses
- filtros
- códigos de erro
- regras de negócio

## 2. Estrutura da API

A API foi organizada em dois recursos principais:

- Projetos
- Tarefas

Um projeto pode possuir várias tarefas. Toda tarefa pertence obrigatoriamente a um projeto.

## 3. Identificadores

Todos os identificadores usam UUID/GUID.

Os campos `id`, `createdAt` e `completedAt` são controlados pela aplicação, não pelo cliente.

## 4. Status

Projeto possui os status:

- active
- archived

Tarefa possui os status:

- pending
- in_progress
- done

A transição de status da tarefa segue o fluxo:

```text
pending -> in_progress -> done