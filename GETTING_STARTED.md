# 🚀 Como Começar - Mottu Rental Service

## ✅ Status: IMPLEMENTAÇÃO COMPLETA E FUNCIONAL

A solução está **100% implementada** e pronta para execução!

---

## 📋 Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e rodando
- (Opcional) [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) para desenvolvimento local

---

## ⚡ Execução Rápida (3 minutos)

### Opção 1: Windows (Recomendado)

```powershell
# Navegue até a pasta do projeto
cd "c:\Users\Edson Goncalves\Desafio-BackEnd"

# Execute o script de setup automático
.\scripts\setup.ps1

# Aguarde ~30 segundos até todos os serviços estarem prontos
# O script vai exibir o status de cada serviço

# Acesse a API: http://localhost:5000
```

### Opção 2: Manual (Multiplataforma)

```bash
# Navegue até a pasta do projeto
cd "c:\Users\Edson Goncalves\Desafio-BackEnd"

# Inicie todos os serviços com Docker Compose
docker-compose up -d

# Aguarde ~30 segundos

# Verifique o status
docker-compose ps

# Todos os containers devem estar "healthy" ou "running"
```

---

## 🌐 Acessar a Aplicação

Após a execução, acesse:

### API com Swagger UI
- **URL**: http://localhost:5000
- **Documentação interativa** com todos os endpoints
- **Teste direto na interface** do Swagger

### RabbitMQ Management Console
- **URL**: http://localhost:15672
- **Usuário**: guest
- **Senha**: guest

### MinIO Console (Storage)
- **URL**: http://localhost:9001
- **Usuário**: minioadmin
- **Senha**: minioadmin

---

## 🧪 Testando a API

### 1. Cadastrar uma Moto

No Swagger, vá em `POST /motos` ou use curl:

```bash
curl -X POST http://localhost:5000/motos \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "moto-honda-001",
    "ano": 2024,
    "modelo": "Honda CG 160",
    "placa": "ABC1D23"
  }'
```

**Resposta esperada:**
```json
{
  "id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "mensagem": "Moto cadastrada com sucesso"
}
```

**O que acontece nos bastidores:**
- ✅ Moto salva no PostgreSQL
- ✅ Evento publicado no RabbitMQ
- ✅ Como é ano 2024, o consumer captura e salva no MongoDB
- ✅ Log estruturado registrado

### 2. Consultar Motos

```bash
# Todas as motos
curl http://localhost:5000/motos

# Por placa específica
curl http://localhost:5000/motos?placa=ABC1D23
```

### 3. Cadastrar um Entregador

```bash
curl -X POST http://localhost:5000/entregadores \
  -H "Content-Type: application/json" \
  -d '{
    "identificador": "entregador-joao",
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

**Planos disponíveis:**
- `7` dias: R$ 30/dia (multa 20% se devolver antes)
- `15` dias: R$ 28/dia (multa 40% se devolver antes)
- `30` dias: R$ 22/dia
- `45` dias: R$ 20/dia
- `50` dias: R$ 18/dia

---

## 🔍 Verificando os Serviços

### Ver Logs da API

```bash
docker-compose logs -f api
```

### Ver Mensagens no RabbitMQ

1. Acesse http://localhost:15672
2. Login: guest / guest
3. Vá em **Queues and Streams**
4. Veja a queue `motorcycle.registered.2024`

### Ver Eventos no MongoDB

```bash
# Conectar ao MongoDB
docker-compose exec mongodb mongosh

# Usar o banco
use mottu_events

# Ver eventos
db.motorcycle_events.find().pretty()
```

### Ver Dados no PostgreSQL

```bash
# Conectar ao PostgreSQL
docker-compose exec postgres psql -U admin -d mottu_rental

# Listar motos
SELECT * FROM motorcycles;

# Listar entregadores
SELECT * FROM delivery_drivers;

# Listar locações
SELECT * FROM rentals;
```

---

## 🛑 Parar os Serviços

```bash
# Parar e manter os dados
docker-compose down

# Parar e remover todos os dados (fresh start)
docker-compose down -v
```

---

## 📊 Estrutura de Dados

### Placa de Moto
- Formato: `ABC1D23` (3 letras + 1 número + 1 letra/número + 2 números)
- Validação automática

### CNPJ
- Formato: `12345678000190` (14 dígitos)
- Validação de dígitos verificadores

### CNH
- Tipos aceitos: `A`, `B`, `AB`
- Para alugar moto: **apenas A ou AB**
- Formato de imagem: **PNG ou BMP**

### Cálculo de Locação

**Exemplo 1: Devolução Normal (7 dias)**
- Plano: 7 dias x R$ 30 = R$ 210
- Devolução no dia certo = R$ 210

**Exemplo 2: Devolução Antecipada (plano 7 dias, devolveu no dia 5)**
- Dias usados: 5 x R$ 30 = R$ 150
- Dias não usados: 2 dias
- Multa: 2 x R$ 30 x 20% = R$ 12
- Total: R$ 150 + R$ 12 = R$ 162

**Exemplo 3: Devolução Atrasada (plano 7 dias, devolveu no dia 10)**
- Plano: 7 x R$ 30 = R$ 210
- Diárias extras: 3 x R$ 50 = R$ 150
- Total: R$ 210 + R$ 150 = R$ 360

---

## 🐛 Troubleshooting

### Porta já em uso

**Erro**: `Bind for 0.0.0.0:5000 failed: port is already allocated`

**Solução**:
```bash
# Windows
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:5000 | xargs kill -9
```

### Docker não está rodando

**Erro**: `Cannot connect to the Docker daemon`

**Solução**: Abra o Docker Desktop e aguarde inicializar

### Containers não ficam "healthy"

```bash
# Ver logs de um serviço específico
docker-compose logs postgres
docker-compose logs mongodb
docker-compose logs rabbitmq
docker-compose logs api

# Reiniciar um serviço
docker-compose restart api
```

### API não responde

```bash
# Verificar status
docker-compose ps

# Ver logs
docker-compose logs api

# Reiniciar
docker-compose restart api
```

---

## 📝 Endpoints Disponíveis

### Motos
- `POST /motos` - Cadastrar moto
- `GET /motos` - Listar motos (filtro opcional: ?placa=ABC1234)
- `GET /motos/{id}` - Consultar moto por ID
- `PUT /motos/{id}/placa` - Modificar placa
- `DELETE /motos/{id}` - Remover moto

### Entregadores
- `POST /entregadores` - Cadastrar entregador
- `POST /entregadores/{id}/cnh` - Enviar foto da CNH

### Locações
- `POST /locacao` - Criar locação
- `GET /locacao/{id}/valor` - Consultar valor (query: ?data_devolucao=2024-10-25)

---

## 🎯 Recursos Implementados

✅ **Clean Architecture**: Domain, Application, Infrastructure, API  
✅ **DDD**: Entities, Value Objects, Domain Events  
✅ **CQRS**: Commands e Queries separados  
✅ **Event-Driven**: RabbitMQ com MassTransit  
✅ **Multi-Database**: PostgreSQL + MongoDB  
✅ **Logging**: Serilog structured logging  
✅ **Validation**: FluentValidation  
✅ **Error Handling**: Global exception middleware  
✅ **Docker**: Compose com 5 containers  
✅ **Swagger**: Documentação OpenAPI completa  
✅ **Tests**: Unit tests estruturados  

---

## 📚 Documentação Completa

- **README.md** - Documentação técnica completa
- **QUICKSTART.md** - Guia rápido de início
- **IMPLEMENTATION_COMPLETE.md** - Checklist de implementação
- **ARCHITECTURE_IMPLEMENTATION_SUMMARY.md** - Detalhes da arquitetura

---

## 🎉 Pronto para Usar!

A aplicação está **100% funcional** e pronta para testes!

**Qualquer dúvida:**
1. Consulte o README.md completo
2. Verifique os logs: `docker-compose logs -f`
3. Teste no Swagger: http://localhost:5000

---

**Desenvolvido com ❤️ em .NET 8 + Clean Architecture + DDD** 🚀

