# 📑 Índice de Navegação - Sistema de Aluguel de Motos

## 🚀 COMEÇAR AQUI

Se você é novo no projeto, siga esta ordem:

1. **[RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md)** ⭐  
   → Visão geral completa do projeto em 3 minutos

2. **[GETTING_STARTED.md](GETTING_STARTED.md)** ⚡  
   → Como executar o projeto (passo a passo)

3. **[README.md](README.md)** 📘  
   → Documentação técnica completa

---

## 📚 Documentação

### Para Executar o Projeto
- **[GETTING_STARTED.md](GETTING_STARTED.md)** - Guia completo de execução
- **[QUICKSTART.md](QUICKSTART.md)** - Guia rápido (3 minutos)
- **[scripts/setup.ps1](scripts/setup.ps1)** - Script automático Windows
- **[scripts/setup.sh](scripts/setup.sh)** - Script automático Linux/Mac

### Para Entender a Arquitetura
- **[ARCHITECTURE_IMPLEMENTATION_SUMMARY.md](ARCHITECTURE_IMPLEMENTATION_SUMMARY.md)** - Detalhes da arquitetura
- **[complete-backend-architecture.plan.md](complete-backend-architecture.plan.md)** - Plano arquitetural completo
- **[IMPLEMENTATION_COMPLETE.md](IMPLEMENTATION_COMPLETE.md)** - Checklist de implementação

### Para Gerentes/Liderança
- **[RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md)** - Visão executiva e métricas

### Documentação Técnica
- **[README.md](README.md)** - Documentação completa do projeto

---

## 🏗️ Estrutura do Código

### Camada de Domínio (`src/Sistema.Domain/`)
```
📦 Sistema.Domain
├── 📂 Entities/              - Motorcycle, DeliveryDriver, Rental
├── 📂 ValueObjects/          - LicensePlate, CNPJ, CNH, RentalPlan
├── 📂 Events/                - Domain Events
├── 📂 Interfaces/            - Repository interfaces
├── 📂 Specifications/        - Business rules
└── 📂 Exceptions/            - Domain exceptions
```

**Conceitos**: DDD, Entities, Value Objects, Domain Events

### Camada de Aplicação (`src/Sistema.Application/`)
```
📦 Sistema.Application
├── 📂 Commands/              - Write operations (CQRS)
│   ├── Motorcycles/          - RegisterMotorcycle, UpdatePlate, Delete
│   ├── DeliveryDrivers/      - RegisterDriver, UploadCNH
│   └── Rentals/              - CreateRental
├── 📂 Queries/               - Read operations (CQRS)
│   ├── Motorcycles/          - GetMotorcycles, GetById
│   └── Rentals/              - GetRentalCost
├── 📂 DTOs/                  - Data Transfer Objects
├── 📂 Validators/            - FluentValidation rules
└── 📂 Behaviors/             - MediatR pipelines
```

**Conceitos**: CQRS, Mediator Pattern, Validation

### Camada de Infraestrutura (`src/Sistema.Infrastructure/`)
```
📦 Sistema.Infrastructure
├── 📂 Persistence/
│   ├── PostgreSQL/           - EF Core DbContext, Repositories
│   └── MongoDB/              - MongoDB context, Event storage
├── 📂 Messaging/             - RabbitMQ + MassTransit
│   └── Consumers/            - Event consumers
└── 📂 Storage/               - File storage service
```

**Conceitos**: Repository Pattern, EF Core, Event-Driven

### Camada de API (`src/Sistema.API/`)
```
📦 Sistema.API
├── 📂 Controllers/           - REST endpoints
│   ├── MotorcyclesController.cs
│   ├── DeliveryDriversController.cs
│   └── RentalsController.cs
├── 📂 Middleware/            - Exception handling
├── Program.cs                - App configuration
└── appsettings.json          - Configuration
```

**Conceitos**: REST API, Swagger, Middleware

### Camada Compartilhada (`src/Sistema.Shared/`)
```
📦 Sistema.Shared
├── 📂 Results/               - Result Pattern
└── 📂 Extensions/            - Helper methods
```

---

## 🧪 Testes

### Testes Unitários (`tests/Sistema.UnitTests/`)
```
📦 Sistema.UnitTests
└── 📂 Domain/
    ├── Entities/             - MotorcycleTests.cs
    └── ValueObjects/         - LicensePlateTests, RentalPlanTests
```

### Testes de Integração (`tests/Sistema.IntegrationTests/`)
Estrutura pronta para expansão

### Testes Arquiteturais (`tests/Sistema.ArchitectureTests/`)
Estrutura pronta para validação de regras arquiteturais

---

## 🐳 Docker e DevOps

### Arquivos Docker
- **[Dockerfile](Dockerfile)** - Build multi-stage da API
- **[docker-compose.yml](docker-compose.yml)** - Orquestração completa
- **[.dockerignore](.dockerignore)** - Exclusões de build

### Serviços no Docker Compose
1. **api** - API .NET 8 (porta 5000)
2. **postgres** - PostgreSQL 16 (porta 5432)
3. **mongodb** - MongoDB 7 (porta 27017)
4. **rabbitmq** - RabbitMQ 3 + Management (portas 5672, 15672)
5. **minio** - MinIO Storage (portas 9000, 9001)

---

## 📡 Endpoints da API

### Motos
- `POST /motos` - Cadastrar
- `GET /motos` - Listar
- `GET /motos/{id}` - Buscar por ID
- `PUT /motos/{id}/placa` - Atualizar placa
- `DELETE /motos/{id}` - Remover

### Entregadores
- `POST /entregadores` - Cadastrar
- `POST /entregadores/{id}/cnh` - Upload CNH

### Locações
- `POST /locacao` - Criar
- `GET /locacao/{id}/valor` - Consultar valor

**Swagger**: http://localhost:5000

---

## 🎯 Comandos Úteis

### Executar o Projeto
```bash
# Windows
.\scripts\setup.ps1

# Linux/Mac
chmod +x scripts/setup.sh && ./scripts/setup.sh

# Manual
docker-compose up -d
```

### Ver Logs
```bash
docker-compose logs -f api
docker-compose logs -f postgres
docker-compose logs -f rabbitmq
```

### Compilar
```bash
dotnet build Sistema.RentalService.sln
```

### Testes
```bash
dotnet test
dotnet test tests/Sistema.UnitTests
```

### Parar Serviços
```bash
docker-compose down
docker-compose down -v  # Remove volumes
```

---

## 🔑 Conceitos-Chave

### Arquitetura
- **Clean Architecture**: Separação em camadas independentes
- **DDD**: Domain-Driven Design com entities e value objects
- **CQRS**: Commands e Queries separados
- **Event-Driven**: Comunicação via eventos

### Design Patterns
1. Repository Pattern
2. Unit of Work
3. Mediator (MediatR)
4. Result Pattern
5. Strategy Pattern
6. Factory Pattern
7. Specification Pattern
8. Domain Events
9. Value Objects
10. Dependency Injection

### Princípios SOLID
- **S**ingle Responsibility
- **O**pen/Closed
- **L**iskov Substitution
- **I**nterface Segregation
- **D**ependency Inversion

---

## 📊 Métricas Rápidas

- **Projetos**: 8 (5 src + 3 tests)
- **Linhas de Código**: ~8,000
- **Design Patterns**: 10+
- **Endpoints API**: 9
- **Containers Docker**: 5
- **Documentação**: 800+ linhas

---

## 🆘 Troubleshooting

### Problemas Comuns

**API não inicia**
```bash
docker-compose logs api
docker-compose restart api
```

**Porta em uso**
```bash
# Windows
netstat -ano | findstr :5000

# Linux/Mac
lsof -ti:5000 | xargs kill -9
```

**Database não conecta**
```bash
docker-compose ps
docker-compose logs postgres
```

**Ver todos os logs**
```bash
docker-compose logs -f
```

---

## 📖 Leitura Recomendada por Perfil

### Desenvolvedor Backend
1. [README.md](README.md) - Documentação técnica
2. [ARCHITECTURE_IMPLEMENTATION_SUMMARY.md](ARCHITECTURE_IMPLEMENTATION_SUMMARY.md) - Arquitetura
3. Código em `src/Sistema.Domain/` - Domain layer

### DevOps / Infraestrutura
1. [docker-compose.yml](docker-compose.yml) - Configuração de containers
2. [Dockerfile](Dockerfile) - Build da aplicação
3. [GETTING_STARTED.md](GETTING_STARTED.md) - Deploy local

### Tech Lead / Arquiteto
1. [ARCHITECTURE_IMPLEMENTATION_SUMMARY.md](ARCHITECTURE_IMPLEMENTATION_SUMMARY.md)
2. [complete-backend-architecture.plan.md](complete-backend-architecture.plan.md)
3. Estrutura de camadas em `src/`

### Gerente / Product Owner
1. [RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md) - Visão executiva
2. [QUICKSTART.md](QUICKSTART.md) - Como testar rápido

### QA / Tester
1. [GETTING_STARTED.md](GETTING_STARTED.md) - Como executar
2. Swagger UI: http://localhost:5000
3. Testes em `tests/`

---

## 🎯 Próximos Passos

### Para Começar
1. Leia [RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md)
2. Execute com [GETTING_STARTED.md](GETTING_STARTED.md)
3. Teste no Swagger: http://localhost:5000

### Para Desenvolver
1. Estude a arquitetura em [ARCHITECTURE_IMPLEMENTATION_SUMMARY.md](ARCHITECTURE_IMPLEMENTATION_SUMMARY.md)
2. Navegue pelo código em `src/`
3. Execute os testes: `dotnet test`

### Para Produção
1. Configure autenticação JWT
2. Configure rate limiting
3. Configure monitoring (Prometheus/Grafana)
4. Configure CI/CD pipeline

---

## 📞 Links Rápidos

- **Swagger UI**: http://localhost:5000
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **MinIO Console**: http://localhost:9001 (minioadmin/minioadmin)

---

## ✅ Checklist de Uso

- [ ] Li o RESUMO_EXECUTIVO.md
- [ ] Executei o projeto com docker-compose up -d
- [ ] Acessei o Swagger (http://localhost:5000)
- [ ] Testei cadastrar uma moto
- [ ] Testei cadastrar um entregador
- [ ] Testei criar uma locação
- [ ] Vi os eventos no RabbitMQ
- [ ] Explorei a arquitetura do código

---

**🎉 Tudo pronto para começar! Escolha seu caminho acima e boa jornada! 🚀**

---

**Desenvolvido por Edson Gonçalves com ❤️ em .NET 8 + Clean Architecture + DDD**

