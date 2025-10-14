# simplified-picpay

Este projeto é uma **implementação simplificada da API do PicPay**, desenvolvida em **.NET**.  
Foi inspirado em um <a href="https://github.com/PicPay/picpay-desafio-backend">**teste técnico real do PicPay para desenvolvedores backend**</a>, criado por iniciativa própria com o objetivo de praticar com PostgreSQL, arquitetura limpa, organização de código e integração entre camadas.

---

## Tecnologias Utilizadas

![.NET](https://img.shields.io/badge/DotNet-512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=ffffff)
![C#](https://img.shields.io/badge/CSharp-512BD4.svg?style=for-the-badge&logo=sharp&logoColor=ffffff)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-512BD4.svg?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-2496ED.svg?style=for-the-badge)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-396c94.svg?style=for-the-badge)
![Postman](https://img.shields.io/badge/Postman-ff713d.svg?style=for-the-badge&logo=postman&logoColor=ffffff)
![Swagger](https://img.shields.io/badge/Swagger-%85ea2d.svg?style=for-the-badge&logo=swagger&logoColor=ffffff)
![JWT](https://img.shields.io/badge/JWT-1b1b33.svg?style=for-the-badge&logoColor=ffffff)

---

## Funcionalidades principais

- Cadastro e autenticação de usuários (usuário comum e lojista);
    - Usuário comum utiliza CPF (11 caracteres);
    - Lojista utiliza CNPJ (14 caracteres);
    - CPF/CNPJ e e-mail são únicos no sistema;
    - As senhas são armazenadas de forma segura (hash);

- Transferência de valores entre contas;
    - Usuários comuns podem enviar e receber transferências;
    - Lojistas podem apenas receber;
    - Antes da transferência, o sistema valida saldo e tipo de conta;
    - As operações utilizam o campo public_id para evitar exposição dos IDs internos;

- Validação de autorização externa:
    - Antes de concluir a transferência, a aplicação consulta um serviço externo mockado:
        GET https://util.devi.tools/api/v2/authorize;
    - A transação só é concluída se o serviço autorizar a operação;

- Transação com rollback:
    - Toda operação de transferência é executada dentro de uma transação atômica;
    - Em caso de qualquer falha (ex: autorização negada, exceção), o valor é revertido automaticamente para o pagador;

## Alteração no modelo de endpoint para realizar transação
O modelo original de transação utilizava o ID (chave primária) das contas (accounts), o que poderia representar um risco de segurança, já que exporia identificadores diretos do banco de dados nas requisições.

Para contornar isso, implementei um novo campo chamado public_id na tabela de contas.
Esse campo serve como um identificador público único para cada conta, permitindo realizar operações sem revelar o id interno.

Endpoint original (proposto)

```bash
POST
{
  "value": 100.0,
  "payer": 4,
  "payee": 15
}
```

Endpoint implementado

```bash
POST
{
  "payeepublicid": "pay_f750c25a3aab46bd8cfb13b99f1777c5", 
  "value": 10
}
```

Dessa forma, os IDs internos nunca são expostos.
A aplicação obtém automaticamente o public_id do payer (usuário logado) a partir do token JWT, e apenas o public_id do destinatário (payee) é enviado na requisição.
Isso garante uma comunicação mais segura e desacoplada da estrutura interna do banco de dados.

## Estrutura do Projeto

```bash
simplified-picpay/
 ├── Controllers/           # Controladores da API
 ├── Data/                  # DbContext e Mapeamentos
 ├── DTOs/                  # Data Transfer Objects
 ├── Enums/                 # Enumerações (tipos de conta)
 ├── Exceptions/            # Exceções personalizadas
 ├── Models/                # Models da aplicação
 ├── Repositories/          # Acesso à base de dados
    ├── Abstractions/          # Interfaces dos repositories
 ├── Services/              # Regras de negócio e lógica da aplicação
    ├── Abstractions/          # Interfaces dos services
 ├── Views/                 # Modelos de retorno
 ├── Program.cs             # Ponto de entrada da aplicação
 └── simplified-picpay.csproj
```

## Modelagem das entidades no Banco de dados.

- Account
```bash
    - id (uuid)
    - full_name (varchar)
    - display_name (varchar)
    - public_id (varchar)
    - email (varchar)
    - password_hash (varchar)
    - current_balance (numeric 18,2)
    - account_type (varchar)
    - document (varchar)
    - is_active (boolean)
```

- Transaction
```bash
    - id (uuid)
    - payer_id (uuid)
    - payer_public_id (varchar)
    - payee_id (uuid)
    - payee_public_id (varchar)
    - value (numeric 18,2)
```

## Endpoints

### Home
| Método | Rota                   | Descrição                           | Autenticação |
|--------|------------------------|-------------------------------------|--------------|
| GET    | `/`   | Retorna status da API     | ❌           |

### Accounts

| Método | Rota                   | Descrição                           | Autenticação |
|--------|------------------------|-------------------------------------|--------------|
| POST    | `/accounts/login`   | Realiza login do usuário     | ❌           |
| POST    | `/accounts/search`   | Busca conta pelo displayName     | ✅           |
| POST    | `/accounts`   | Cria uma nova conta (usuário comum ou lojista)     | ❌           |
| PUT    | `/accounts`   | Atualiza informações da conta     | ✅           |
| PUT    | `/accounts/add-founds`   | Adiciona saldo à conta     | ✅           |
| PUT    | `/accounts/remove-founds`   | Remove saldo da conta     | ✅           |
| PUT    | `/accounts/disable-account`   | Desativa a conta logada     | ✅           |

### Transactions

| Método | Rota                   | Descrição                           | Autenticação |
|--------|------------------------|-------------------------------------|--------------|
| POST    | `/transactions`   | Cria uma nova transação entre contas     | ✅           |
| GET    | `/transactions`   | Lista todas as transações do usuário logado     | ✅           |
| GET    | `/transactions/{id}`   | Retorna detalhes de uma transação específica     | ✅           |
| GET    | `/transactions/received`   | Lista transações recebidas pelo usuário logado     | ✅           |
| GET    | `/transactions/paid`   | Lista transações enviadas pelo usuário logado     | ✅           |

### Pré requisitos para rodar localmente

- Ter o <a href="https://dotnet.microsoft.com/pt-br/download/dotnet/9.0">.NET 9.0</a> na máquina.

- Ter o PostgreSQL rodando localmente.

- Ter instalado o Entity Framework em sua máquina.
    ```bash
    dotnet tool install --global dotnet-ef
    ```

## Para executar localmente

- Clonar o repositório
```bash
git clone git@github.com:CostaDenis/simplified-picpay.git
```

- Alterar o caminho do banco no arquivo 'appsettings.Development.json'
```bash
"DefaultConnection": "Host=localhost;Port=5432;Database=simplified_picpay;Username=postgres;Password=1q2w3e4r@#$;"
```

- Aplicar migrações no seu banco de dados local.
```bash
dotnet ef database update
```

- Executar o projeto
```bash
dotnet watch run
```

## Observações finais

### Autenticação
A maioria dos endpoints da API requer autenticação utilizando Bearer Token. Após realizar o login com sucesso, a API retorna um token JWT que representa a sessão do usuário autenticado.

### Mock de notificação pelo Email indisponível 
Durante o desenvolvimento, o mock proposto no desafio estava indisponível. Portanto, é bem provável que ele continue inacessível caso você tente testar a aplicação.

```bash
_ = _notifyService.SendNotificationAsync(
        email: payee.Email,
        message: $"Você recebeu {value} de {payer.DisplayName}!",
        cancellationToken
        );
```

### Testando a aplicação
Para testar a API, utilizei o Postman. Todas as requisições que utilizei estão disponíveis na pasta Postman. Nela, há um arquivo .json que pode ser importado diretamente no Postman para reproduzir a coleção de testes que usei.

Além disso, o Swagger também foi adicionado ao projeto, oferecendo uma alternativa prática para explorar e testar os endpoints da API.
