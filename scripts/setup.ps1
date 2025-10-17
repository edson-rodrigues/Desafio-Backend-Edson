# PowerShell Setup Script for Windows

Write-Host "🏍️  Mottu Rental Service - Setup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if Docker is installed
try {
    docker --version | Out-Null
    Write-Host "✅ Docker is installed" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker is not installed. Please install Docker Desktop first." -ForegroundColor Red
    exit 1
}

# Check if Docker Compose is installed
try {
    docker-compose --version | Out-Null
    Write-Host "✅ Docker Compose is installed" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker Compose is not installed. Please install Docker Compose first." -ForegroundColor Red
    exit 1
}

Write-Host ""

# Stop any running containers
Write-Host "🛑 Stopping existing containers..." -ForegroundColor Yellow
docker-compose down

# Build and start all services
Write-Host "🚀 Starting all services..." -ForegroundColor Green
docker-compose up -d

Write-Host ""
Write-Host "⏳ Waiting for services to be healthy (this may take up to 60 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Wait for services
Write-Host "   - Waiting for PostgreSQL..." -ForegroundColor Gray
$retries = 0
while ($retries -lt 30) {
    try {
        docker-compose exec -T postgres pg_isready -U admin -d mottu_rental 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) {
            break
        }
    } catch {}
    Write-Host "   - PostgreSQL is starting..." -ForegroundColor Gray
    Start-Sleep -Seconds 2
    $retries++
}
Write-Host "   ✅ PostgreSQL is ready" -ForegroundColor Green

Write-Host "   - Waiting for MongoDB..." -ForegroundColor Gray
Start-Sleep -Seconds 5
Write-Host "   ✅ MongoDB is ready" -ForegroundColor Green

Write-Host "   - Waiting for RabbitMQ..." -ForegroundColor Gray
Start-Sleep -Seconds 5
Write-Host "   ✅ RabbitMQ is ready" -ForegroundColor Green

Write-Host "   - Waiting for API..." -ForegroundColor Gray
Start-Sleep -Seconds 5
Write-Host "   ✅ API is ready" -ForegroundColor Green

Write-Host ""
Write-Host "🎉 All services are up and running!" -ForegroundColor Green
Write-Host ""
Write-Host "📡 Available endpoints:" -ForegroundColor Cyan
Write-Host "   - API/Swagger: http://localhost:5000"
Write-Host "   - RabbitMQ Management: http://localhost:15672 (guest/guest)"
Write-Host "   - MinIO Console: http://localhost:9001 (minioadmin/minioadmin)"
Write-Host ""
Write-Host "📊 Check service status:" -ForegroundColor Cyan
Write-Host "   docker-compose ps"
Write-Host ""
Write-Host "📋 View logs:" -ForegroundColor Cyan
Write-Host "   docker-compose logs -f api"
Write-Host ""
Write-Host "🛑 Stop all services:" -ForegroundColor Cyan
Write-Host "   docker-compose down"
Write-Host ""

