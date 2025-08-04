
# Blog Simples API

Bem-vindo à API do Blog Simples\! Este é o backend para um sistema de blog, construído com .NET e C\#, seguindo princípios de arquitetura limpa e design de software moderno.

O projeto permite a autenticação de usuários, gerenciamento completo de postagens (CRUD) e notifica todos os clientes conectados em tempo real sobre novas publicações através de WebSockets.

## ✨ Funcionalidades

  - **Autenticação de Usuários:** Sistema de registro e login seguro utilizando JSON Web Tokens (JWT).
  - **Gerenciamento de Posts (CRUD):** Usuários autenticados podem criar, ler, atualizar e deletar suas próprias postagens.
  - **Visualização Pública:** Qualquer visitante pode visualizar a lista de posts existentes.
  - **Notificações em Tempo Real:** Comunicação via WebSockets para informar sobre novos posts assim que são publicados.
  - **Arquitetura Organizada:** Código estruturado em camadas (`Core`, `Application`, `Infrastructure`, `Api`) para promover a separação de responsabilidades (SoC) e facilitar a manutenção.

## 🚀 Tecnologias e Arquitetura

  - **Framework:** .NET 8
  - **Linguagem:** C\#
  - **Arquitetura:** Monolítica com Separação de Responsabilidades, baseada nos princípios de Clean Architecture.
  - **Princípios de Design:** SOLID (com ênfase em SRP e DIP).
  - **Banco de Dados:** Entity Framework Core 8 para o ORM, interagindo com SQL Server.
  - **Autenticação:** ASP.NET Core Identity para gerenciamento de usuários, combinado com JWT para autenticação de API.
  - **Comunicação Real-Time:** WebSockets nativo do ASP.NET Core.

## 📋 Pré-requisitos

Antes de começar, garanta que você tenha o seguinte software instalado na sua máquina:

  - **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**: O kit de desenvolvimento para compilar e rodar o projeto.
  - **[SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)**: A versão Express ou Developer é suficiente para o ambiente local.
  - **[Git](https://git-scm.com/downloads)**: Para clonar o repositório.
  - **Um Cliente de API**: Recomenda-se **[Postman](https://www.postman.com/downloads/)** ou **[Insomnia](https://insomnia.rest/download)** para testar os endpoints da API.

## ⚙️ Configuração e Instalação

Siga estes passos para configurar e executar o projeto em uma nova máquina.

**1. Clone o Repositório**

Abra um terminal ou prompt de comando e clone o código-fonte do projeto.

```bash
git clone <URL_DO_SEU_REPOSITORIO>
cd <NOME_DA_PASTA_DO_PROJETO>
```

**2. Restaure as Ferramentas .NET**

Este projeto utiliza ferramentas locais do .NET (como o `dotnet-ef`). Restaure-as com o seguinte comando:

```bash
dotnet tool restore
```

**3. Configure a Connection String**

Este é o passo mais importante para conectar ao seu banco de dados local.

  - Abra o arquivo `Blog.Api/appsettings.json`.

  - Encontre a seção `ConnectionStrings`.

  - Altere o valor de `DefaultConnection` para apontar para a **sua instância local do SQL Server**.

      * **Exemplo para Autenticação do Windows (mais comum localmente):**

        ```json
        "DefaultConnection": "Server=NOME_DO_SEU_SERVIDOR\\SQLEXPRESS;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True"
        ```

        *Substitua `NOME_DO_SEU_SERVIDOR\\SQLEXPRESS` pelo nome da sua instância SQL.*

      * **Exemplo para Autenticação do SQL Server:**

        ```json
        "DefaultConnection": "Server=NOME_DO_SEU_SERVIDOR\\SQLEXPRESS;Database=BlogDb;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True"
        ```

**4. Aplique as Migrations (Crie o Banco de Dados)**

Com a connection string configurada, este comando irá criar o banco de dados `BlogDb` e todas as tabelas necessárias.

```bash
dotnet ef database update --startup-project Blog.Api
```

## ▶️ Executando a Aplicação

Após a configuração, você está pronto para iniciar o servidor da API.

1.  No terminal, na pasta raiz do projeto, execute:
    ```bash
    dotnet run --project Blog.Api
    ```
2.  O terminal mostrará as URLs onde a aplicação está rodando (ex: `https://localhost:7142`).
3.  Acesse a URL `/swagger` no seu navegador (ex: `https://localhost:7142/swagger`) para ver a documentação interativa da API.

## 🧪 Como Testar a API

Para verificar se tudo está funcionando, siga este fluxo de teste:

1.  **Abra um Cliente WebSocket** (ex: extensão "Simple WebSocket Client" para Chrome) e conecte-se a `ws://localhost:PORTA/ws` (use a porta HTTP do seu terminal).
2.  **Use o Postman** para fazer as seguintes requisições:
    a. **`POST /api/auth/register`**: Crie um novo usuário.
    b. **`POST /api/auth/login`**: Faça login com o usuário criado e **copie o token JWT** da resposta.
    c. **`POST /api/posts`**: Crie um novo post. Lembre-se de adicionar o token no header de autorização (`Authorization: Bearer SEU_TOKEN_AQUI`).
3.  **Verifique o Cliente WebSocket**: Ao criar o post, uma mensagem de notificação em JSON deverá aparecer instantaneamente no seu cliente WebSocket.

Se a mensagem apareceu, a aplicação está 100% funcional\!

## 📂 Estrutura do Projeto

```
BlogSolution/
├── Blog.Core/              # Entidades de negócio e interfaces de repositório (o coração da aplicação).
├── Blog.Application/       # Lógica de negócio, serviços e casos de uso.
├── Blog.Infrastructure/    # Implementações de acesso a dados (EF Core) e serviços externos.
└── Blog.Api/               # Ponto de entrada da API (Controllers, DTOs, Middlewares e configuração).
```
