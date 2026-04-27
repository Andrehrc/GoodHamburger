<h1 align="center">
<br>
  <img src="https://www.stgen.com.br/images/logo_stgenetics_250.png" alt="STGenetics" width="400">

<br />
 🍔 Good Hamburger — Sistema de Pedidos
</h1>

## Tools
API REST em **ASP.NET Core 8** + frontend **Blazor WebAssembly**, com persistência em **PostgreSQL** via Entity Framework Core.

- 💻 **ASP.NET Core 6** — Framework para desenvolvimento web
- 🗃️ **Entity Framework Core** — ORM para simplificar o acesso ao banco de dados
- 💾 **Postgres** — Banco de dados relacional
- 📝 **Swagger** — Documentação Open API 

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL 14+ rodando localmente (ou via Docker)

## Getting started

1. Clone esse repositorio utilizando `git clone https://github.com/Andrehrc/GoodHamburger.git`
2. Acesse o diterório do projeto: `cd GoodHamburger`<br />
3. Rode `dotnet restore` para instalar as dependências<br />

## Prepare o ambiente do banco de dados
4. Certifique-se de ter o Dotnet Ef instalado, que pode ser feito com o comando `dotnet tool install --global dotnet-ef` 
5. Rode o comando `dotnet ef migrations add InitialCreate --project GoodHamburger` para gerar migrations 
6. Se necessário, atualize a string de conexão no arquivo `appsettings.json` para refletir as configurações do seu banco de dados:
  ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=SuaPorta;Database=GoodHamburger;Username=SeuUsuario;Password=SuaSenha"
   }
   ```

## Prepare o ambiente de desenvolvimento e execute a aplicação
7. Caso esteja utilizando Windows, execute o comando `set ASPNETCORE_ENVIRONMENT=Development` para atualizar o ambiente de para desenvolvimento<br />
8. Caso esteja utilizando Mac ou Linux, execute o comando `ASPNETCORE_ENVIRONMENT=Development dotnet run --project BoletosApi` para atualizar o ambiente de para desenvolvimento e iniciar a aplicação<br />

### 1. Iniciar a API

```bash
cd GoodHamburger
dotnet run
```

A API sobe em `http://localhost:5127/`.
As migrations são **aplicadas automaticamente** na inicialização.
Swagger UI: `http://localhost:5127/swagger/index.html`

### 2. Iniciar o Blazor (em outro terminal)

```bash
cd GoodHamburger.App
dotnet run
```

Frontend disponível em `http://localhost:5105/`.

## Decisões de arquitetura

**IOrderService** isola as regras de negócio da API.
**IOrderRepository** isola o repositório.
**PricingService estático** função pura f(items) - pricing, sem estado, 100% testável.
**ItemCodes como text[]** Npgsql suporta arrays nativos do PostgreSQL, sem tabela extra.
**Migrations automáticas** db.Database.Migrate() no startup. Em produção, prefira rodar no pipeline de CI/CD.
**Blazor** — OrderApiService centraliza as chamadas HTTP; validação de duplicata também no cliente para feedback imediato.

## Endpoints da API

| Método   | Rota               | Descrição                   |
|----------|--------------------|-----------------------------|
| GET      | /api/menu          | Lista o cardápio             |
| GET      | /api/orders        | Lista todos os pedidos       |
| GET      | /api/orders/{id}   | Busca pedido por ID          |
| POST     | /api/orders        | Cria um novo pedido          |
| PUT      | /api/orders/{id}   | Atualiza itens de um pedido  |
| DELETE   | /api/orders/{id}   | Remove um pedido             |

## Documentação

A documentação da API está disponível em Swagger após iniciar o projeto.

<br />
<img src="./assets/host.png" />
<br />

## O que ficou de fora

- Autenticação/autorização (fora do escopo)
- Docker Compose para subir API + banco juntos
- Paginação em GET /orders (facilmente adicionável com Skip/Take)
