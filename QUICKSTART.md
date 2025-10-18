# 🚀 Quick Start Guide - Sistema de Aluguel de Motos

## Execução em 3 Passos

### Windows (PowerShell)

```powershell
# 1. Execute o script de setup
.\scripts\setup.ps1

# 2. Aguarde ~30 segundos

# 3. Acesse: http://localhost:5000
```

### Linux/Mac (Bash)

```bash
# 1. Dê permissão ao script
chmod +x scripts/setup.sh

# 2. Execute
./scripts/setup.sh

# 3. Acesse: http://localhost:5000
```

### Manual

```bash
# 1. Inicie todos os serviços
docker-compose up -d

# 2. Aguarde ~30 segundos

# 3. Acesse: http://localhost:5000
```

## 🧪 Testando a API

### 1. Cadastrar uma Moto

```bash
curl -X POST http://localhost:5000/motos \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "moto-001",
    "ano": 2024,
    "modelo": "Honda CG 160",
    "placa": "ABC1D23"
  }'
```

**Resposta esperada:**
```json
{
  "id": "guid-gerado",
  "mensagem": "Moto cadastrada com sucesso"
}
```

### 2. Consultar Motos

```bash
curl http://localhost:5000/motos
```

### 3. Cadastrar um Entregador

```bash
curl -X POST http://localhost:5000/entregadores \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "entregador-001",
    "nome": "João Silva",
    "cnpj": "12345678000190",
    "data_nascimento": "1990-05-15",
    "numero_cnh": "12345678901",
    "tipo_cnh": "A"
  }'
```

### 4. Criar uma Locação

```bash
curl -X POST http://localhost:5000/locacao \
  -H "Content-Type: application/json" \
  -d '{
    "entregador_id": "GUID_DO_ENTREGADOR",
    "moto_id": "GUID_DA_MOTO",
    "data_inicio": "2024-10-18",
    "data_termino": "2024-10-25",
    "data_previsao_termino": "2024-10-25",
    "plano": 7
  }'
```

## 📊 Acessar Interfaces

- **Swagger UI**: http://localhost:5000
- **RabbitMQ Management**: http://localhost:15672
  - User: `guest`
  - Password: `guest`
- **MinIO Console**: http://localhost:9001
  - User: `minioadmin`
  - Password: `minioadmin`

## 🔍 Verificar Logs

```bash
# Todos os serviços
docker-compose logs -f

# Apenas a API
docker-compose logs -f api

# PostgreSQL
docker-compose logs -f postgres

# RabbitMQ
docker-compose logs -f rabbitmq
```

## 🛑 Parar os Serviços

```bash
# Parar e manter os dados
docker-compose down

# Parar e remover dados (fresh start)
docker-compose down -v
```

## ⚙️ Executar Localmente (Sem Docker para API)

```bash
# 1. Inicie apenas a infraestrutura
docker-compose up postgres mongodb rabbitmq minio -d

# 2. Configure as variáveis de ambiente
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=mottu_rental;Username=admin;Password=admin123"

# 3. Execute a API
cd src/Sistema.API
dotnet run

# 4. Acesse: http://localhost:5000
```

## 🧪 Executar Testes

```bash
# Testes unitários
dotnet test tests/Sistema.UnitTests

# Testes de integração
dotnet test tests/Sistema.IntegrationTests

# Todos os testes
dotnet test
```

## 📝 Especificação da API

A API segue as especificações REST com documentação completa via Swagger/OpenAPI.

## 🐛 Troubleshooting

### Erro: "Port already in use"

```bash
# Verifique quais portas estão em uso
docker-compose ps

# Pare os containers conflitantes
docker stop $(docker ps -q)
```

### Erro: "Cannot connect to Docker daemon"

Certifique-se de que o Docker Desktop está rodando.

### API não responde

```bash
# Verifique o status dos containers
docker-compose ps

# Verifique os logs
docker-compose logs api
```

## 🎯 Funcionalidades Implementadas

✅ **Motos**
- Cadastrar moto
- Consultar motos (com filtro por placa)
- Modificar placa
- Remover moto (valida se não tem locações)

✅ **Entregadores**
- Cadastrar entregador
- Upload de CNH (PNG/BMP)
- Validação de CNH tipo A ou AB

✅ **Locações**
- Criar locação
- Calcular valor com multas/diárias extras
- Validações de regras de negócio

✅ **Eventos**
- Publicação no RabbitMQ
- Consumidor para motos 2024
- Armazenamento no MongoDB

✅ **Infraestrutura**
- Clean Architecture
- DDD + CQRS
- PostgreSQL + MongoDB
- RabbitMQ + MassTransit
- Serilog Logging
- Docker Compose completo

## 📞 Suporte

Em caso de dúvidas:
1. Verifique os logs: `docker-compose logs -f`
2. Consulte o README.md completo
3. Verifique o Swagger: http://localhost:5000

---

**Pronto para começar!** 🚀

