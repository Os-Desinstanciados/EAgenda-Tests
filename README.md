# 📅 eAgenda

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![C#](https://img.shields.io/badge/C%23-C%23-239120)
[![Academia do Programador](https://img.shields.io/badge/Academia%20do%20Programador-Fullstack%202026-6f42c1)](https://www.academiadoprogramador.net/inicio)

O eAgenda é uma aplicação web desenvolvida em ASP.NET Core MVC para o gerenciamento de compromissos, contatos, tarefas e despesas pessoais. O sistema permite organizar compromissos, categorizar despesas e acompanhar tarefas e seus itens, além de contar com autenticação de usuários e isolamento dos dados por usuário. O projeto também possui testes automatizados para validação das funcionalidades e regras de negócio implementadas.

## Projeto

Desenvolvido durante o curso **Fullstack 2026** da [Academia do Programador](https://www.academiadoprogramador.net/), com foco na aplicação prática de conceitos de desenvolvimento web, arquitetura de software, persistência de dados e testes automatizados.

## Getting Started

### Prerequisites

Antes de executar o projeto, certifique-se de possuir as seguintes ferramentas instaladas:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/downloads)

### Clone o repositório

Clone o projeto e acesse o diretório da solução:

```bash
git clone https://github.com/Os-Desinstanciados/EAgenda-Tests.git
cd EAgendaTests
```

### Configure o banco de dados

Aplique as migrations existentes para criar e atualizar o banco de dados:

```bash
dotnet ef database update --project ./src/eAgenda.Infra --startup-project ./src/eAgenda.WebApp
```

### Testes

O projeto conta com diferentes níveis de testes automatizados:

| Teste | Tecnologia | Objetivo |
|---|---|---|
| `Unitários` | MSTest | Validar entidades e regras de negócio |
| `Integração` | MSTest + Entity Framework Core | Validar a integração com a camada de persistência |
| `End-to-End (E2E)` | MSTest + Playwright | Validar os principais fluxos através da interface da aplicação |

**Examples:**

🧪 Executar todos os testes:

```bash
dotnet test
```

🔬 Executar somente os testes unitários:

```bash
dotnet test ./tests/eAgenda.Testes.Unidade
```

🔗 Executar somente os testes de integração:

```bash
dotnet test ./tests/eAgenda.Testes.Integracao
```

🌐 Executar somente os testes E2E:

```bash
dotnet test ./tests/eAgenda.Testes.E2E
```

> 💡 **Tip:** Antes de executar os testes E2E pela primeira vez, certifique-se de que os navegadores do Playwright estejam instalados.

### Run the app

Execute a aplicação Web:

```bash
dotnet run --project ./src/eAgenda.WebApp
```

Após a inicialização, acesse no navegador o endereço exibido pelo terminal.

## Funcionalidades

- Gerenciamento de Contatos
- Gerenciamento de Compromissos
- Gerenciamento de Categorias
- Gerenciamento de Despesas
- Gerenciamento de Tarefas
- Gerenciamento de Itens das tarefas
- Acompanhamento do percentual de conclusão das tarefas
- Autenticação de usuários
- Isolamento dos dados por usuário

## Tecnologias

- **ASP.NET Core MVC** — desenvolvimento da aplicação web
- **Entity Framework Core** — persistência e acesso aos dados
- **SQL Server** — banco de dados
- **ASP.NET Core Identity** — autenticação e gerenciamento de usuários
- **AutoMapper** — mapeamento entre entidades, DTOs e ViewModels
- **FluentResults** — tratamento dos resultados das operações
- **Bootstrap** — estilização e responsividade da interface
- **MSTest** — testes automatizados
- **Playwright** — testes End-to-End (E2E)

## Arquitetura

O projeto foi desenvolvido utilizando uma arquitetura em camadas, separando as responsabilidades da aplicação entre **Domínio, Aplicação, Infraestrutura e Apresentação**.

![Domínio](https://img.shields.io/badge/🧠_Domínio-Regras_de_Negócio-blue)

Entidades, validações, regras de negócio e contratos dos repositórios.

![Aplicação](https://img.shields.io/badge/⚙️_Aplicação-Serviços-purple)

Serviços, DTOs e coordenação dos casos de uso da aplicação.

![Infraestrutura](https://img.shields.io/badge/🗄️_Infraestrutura-Persistência-orange)

Persistência de dados, Entity Framework Core, repositórios, configurações ORM e migrations.

![WebApp](https://img.shields.io/badge/🖥️_WebApp-MVC-green)

Interface da aplicação utilizando ASP.NET Core MVC, Controllers, Views, ViewModels e Bootstrap.