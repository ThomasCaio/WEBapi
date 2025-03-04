# WEBapi: Uma API para Gerenciamento de Tarefas (To-Do List)

Este projeto é uma API desenvolvida em C# com o objetivo de estudo e prática dos conceitos de desenvolvimento web com .NET. Ele demonstra a criação de uma API robusta e bem estruturada para gerenciamento de tarefas (To-Do List), utilizando os princípios de Clean Architecture e testes unitários abrangentes.

## Funcionalidades

A API oferece as seguintes funcionalidades:

* **Gerenciamento de Tarefas:**
    * Criação, leitura, atualização e exclusão de tarefas (CRUD).
    * Listagem de todas as tarefas.
    * Busca de tarefas por ID.
* **Gerenciamento de Usuários:**
    * Criação e leitura de usuários.
    * Listagem de todos os usuários.

## Tecnologias Utilizadas

* **C#:** Linguagem de programação principal.
* **.NET:** Framework para desenvolvimento web.
* **ASP.NET Core:** Framework para criação de APIs.
* **Entity Framework Core:** ORM para interação com o banco de dados em memória (In-Memory Database) para testes.
* **xUnit:** Framework para testes unitários.
* **Moq:** Framework para criação de mocks em testes unitários.
* **Clean Architecture:** Arquitetura para organização e separação de responsabilidades.
* **Injeção de Dependência:** Para desacoplamento e melhor testabilidade.

## Arquitetura

O projeto segue os princípios da Clean Architecture, dividindo a aplicação em camadas distintas:

* **Controllers:** Responsáveis por receber as requisições HTTP e retornar as respostas.
* **Services:** Contêm a lógica de negócio da aplicação.
* **Models:** Representam as entidades do domínio.
* **Context:** Responsável pela interação com o banco de dados.
* **Tests:** Contêm os testes unitários para todas as camadas da aplicação.

## Testes

O projeto possui testes unitários abrangentes para todas as camadas da aplicação, garantindo a qualidade e a confiabilidade do código. Os testes utilizam xUnit e Moq para isolar as unidades de código e simular dependências externas.

## Como Executar

1.  Clone o repositório.
2.  Restaure os pacotes NuGet: `dotnet restore`
3.  Execute a API: `dotnet run --project WEBapi/WEBapi.csproj`
4.  Execute os testes: `dotnet test --project WEBapi.Tests/WEBapi.Tests.csproj`


Este projeto foi desenvolvido com o objetivo de estudo e prática.