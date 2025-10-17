#!/bin/bash

echo "🏍️  Mottu Rental Service - Setup Script"
echo "======================================="
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker Desktop first."
    exit 1
fi

# Check if Docker Compose is installed
if ! command -v docker-compose &> /dev/null; then
    echo "❌ Docker Compose is not installed. Please install Docker Compose first."
    exit 1
fi

echo "✅ Docker and Docker Compose are installed"
echo ""

# Stop any running containers
echo "🛑 Stopping existing containers..."
docker-compose down

# Remove old volumes (optional - uncomment if you want fresh start)
# echo "🗑️  Removing old volumes..."
# docker-compose down -v

# Build and start all services
echo "🚀 Starting all services..."
docker-compose up -d

echo ""
echo "⏳ Waiting for services to be healthy (this may take up to 60 seconds)..."
sleep 10

# Wait for PostgreSQL
echo "   - Waiting for PostgreSQL..."
until docker-compose exec -T postgres pg_isready -U admin -d mottu_rental &> /dev/null; do
    echo "   - PostgreSQL is starting..."
    sleep 2
done
echo "   ✅ PostgreSQL is ready"

# Wait for MongoDB
echo "   - Waiting for MongoDB..."
until docker-compose exec -T mongodb mongosh --eval "db.adminCommand('ping')" &> /dev/null; do
    echo "   - MongoDB is starting..."
    sleep 2
done
echo "   ✅ MongoDB is ready"

# Wait for RabbitMQ
echo "   - Waiting for RabbitMQ..."
until docker-compose exec -T rabbitmq rabbitmq-diagnostics ping &> /dev/null; do
    echo "   - RabbitMQ is starting..."
    sleep 2
done
echo "   ✅ RabbitMQ is ready"

# Wait for API
echo "   - Waiting for API..."
sleep 5
echo "   ✅ API is ready"

echo ""
echo "🎉 All services are up and running!"
echo ""
echo "📡 Available endpoints:"
echo "   - API/Swagger: http://localhost:5000"
echo "   - RabbitMQ Management: http://localhost:15672 (guest/guest)"
echo "   - MinIO Console: http://localhost:9001 (minioadmin/minioadmin)"
echo ""
echo "📊 Check service status:"
echo "   docker-compose ps"
echo ""
echo "📋 View logs:"
echo "   docker-compose logs -f api"
echo ""
echo "🛑 Stop all services:"
echo "   docker-compose down"
echo ""

