# HomeVision Orchestrator - Deployment Guide

## Prerequisites

- Docker & Docker Compose installed
- Samsung Tizen Studio (for TV app deployment)
- .NET SDK 8.0 (for local development)
- Node.js 20+ (for local development)

## Quick Start with Docker

### 1. Clone and Configure

```bash
cd c:\Users\holam\Documents\MiProyectosVSCode\ProyectoComputadorTelevisor\HomeVision-Orchestrator

# Copy environment template
copy .env.example .env

# Edit .env with your settings
notepad .env
```

**Important variables to configure:**
- `SAMSUNG_TV_IP`: Your Samsung TV's IP address
- `DB_PASSWORD`: Strong password for SQL Server
- `JWT_SECRET`: Random secret key for JWT tokens

### 2. Start All Services

```bash
docker-compose up --build -d
```

This will start:
- SQL Server (port 1433)
- Redis (port 6379)
- Auth Service (port 5001)
- Orchestration Service (port 5002)
- TV Integration Service (port 3000 + WebSocket 8080)
- API Gateway (port 5000)
- Web Dashboard (port 3000)
- Prometheus (port 9090)
- Grafana (port 3001)

### 3. Verify Services

```bash
# Check all containers are running
docker-compose ps

# View logs
docker-compose logs -f api-gateway

# Test API
curl http://localhost:5000/api/scenes
```

### 4. Access UIs

- **API Documentation**: http://localhost:5000/swagger
- **Web Dashboard**: http://localhost:3000
- **Grafana Monitoring**: http://localhost:3001 (admin/admin)

## Tizen TV App Deployment

### Enable Developer Mode on Samsung TV

1. Press **Home** button on remote
2. Go to **Apps**
3. Press **12345** on the keypad
4. Toggle **Developer mode** to ON
5. Enter your PC's IP address
6. Restart TV

### Install Tizen Studio

1. Download from [developer.samsung.com/tizen](https://developer.samsung.com/tizen)
2. Install Tizen Studio with TV extensions
3. Add Tizen tools to PATH

### Package and Deploy TV App

```bash
cd frontend\tizen-tv-app

# Install dependencies
npm install

# Build (if using webpack)
npm run build

# Package the app
tizen package -t wgt -s [YOUR_CERTIFICATE_NAME]

# Connect to TV
tizen connect [YOUR_TV_IP]

# Install on TV
tizen install -n HomeVision.wgt -t [YOUR_TV_IP]

# Launch app
tizen run -p HomeVisionOrchestrator -t [YOUR_TV_IP]
```

### Update API URL in TV App

Before packaging, edit `frontend/tizen-tv-app/index.html`:

```javascript
// Line ~200, change to your server IP
const API_URL = 'http://YOUR_SERVER_IP:5000/api';
const WS_URL = 'ws://YOUR_SERVER_IP:8080';
```

## Local Development (Without Docker)

### Backend Services

```bash
# Start SQL Server locally or use Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest

# API Gateway
cd backend\api-gateway\HomeVision.Gateway
dotnet run

# Auth Service
cd backend\auth-service\HomeVision.Auth
dotnet run

# TV Integration Service
cd backend\tv-integration-service\tv-service
npm install
npm start
```

### TV App Development

Use Tizen Studio IDE or any web dev tools for the HTML5 app.

## Production Deployment

### Using Kubernetes (Optional)

```bash
# Apply configurations
kubectl apply -f infra/k8s/deployments/
kubectl apply -f infra/k8s/services/

# Check status
kubectl get pods
kubectl get services
```

### Firewall Configuration

Open these ports:
- `5000` - API Gateway
- `5001` - Auth Service
- `8080` - WebSocket

## Troubleshooting

### TV App Can't Connect to API

1. Check TV and server are on same network
2. Verify API URL in `index.html` is correct
3. Test API from browser: `http://YOUR_IP:5000/api/scenes`
4. Check firewall allows connections

### SQL Server Connection Fails

1. Verify password in `.env` matches requirements (uppercase, lowercase, number, symbol)
2. Check container is healthy: `docker-compose logs sqlserver`
3. Try connecting with SQL Server Management Studio

### WebSocket Not Working

1. Check port 8080 is open
2. Verify WS_URL in TV app
3. Check TV Integration Service logs: `docker-compose logs tv-integration-service`

## Monitoring

Access Grafana at http://localhost:3001 to view:
- Service health metrics
- API request rates
- Error rates
- Resource usage

Default credentials: `admin` / `admin`
