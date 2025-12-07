# 🏠 HomeVision Orchestrator

> Convierte tu Samsung Smart TV en el cerebro visual de tu casa inteligente

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Node.js](https://img.shields.io/badge/Node.js-20-green.svg)
![Docker](https://img.shields.io/badge/Docker-ready-blue.svg)

## 📖 Descripción

**HomeVision Orchestrator** transforma tu Samsung UE32T430A (o cualquier Smart TV Samsung compatible con Tizen) en un centro de control domótico visual de pantalla grande, manteniendo intactas todas las funcionalidades originales del televisor (Xbox, Netflix, etc.).

### ✨ Características Principales

- 📺 **Dashboard en TV de 32"** - Control visual de toda tu casa inteligente
- 🎬 **Escenas Automáticas** - "Hola casa", "Modo cine", "Buenas noches"
- 🔔 **Notificaciones Grandes** - Alertas visibles en tu TV cuando algo importante sucede
- 🔄 **Sincronización Multi-Plataforma** - Control desde TV, web y móvil
- 🎮 **No Invasivo** - Xbox y Netflix siguen funcionando perfectamente

### 🏗️ Arquitectura

```
┌─────────────────┐
│  Samsung TV     │ ◄─── Tizen App (HTML5/JS)
│  (Tizen OS)     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐      ┌──────────────────┐
│  API Gateway    │ ◄──► │  Web SPA         │
│  (ASP.NET Core) │      │  (React)         │
└────────┬────────┘      └──────────────────┘
         │
    ┌────┴────┬────────────┬──────────────┐
    ▼         ▼            ▼              ▼
┌────────┐ ┌─────┐  ┌──────────┐  ┌──────────────┐
│  Auth  │ │Rules│  │TV Service│  │  SQL Server  │
│Service │ │Engine│  │(Node.js) │  │   Database   │
└────────┘ └─────┘  └──────────┘  └──────────────┘
```

### 📅 Casos de Uso Diarios

| Hora  | Acción Automatizada                                    |
|-------|-------------------------------------------------------|
| 7:00  | TV enciende + Café + Ajuste de clima                 |
| 18:00 | Llegas + Luces encendidas + Música automática        |
| 20:00 | Modo cine + Netflix + Luces bajas                    |
| 23:00 | Todo apaga + Alarmas activadas                       |

## 🚀 Inicio Rápido

### Prerrequisitos

- Docker & Docker Compose
- .NET SDK 8.0+ (para desarrollo local)
- Node.js 20+ (para desarrollo local)
- Samsung Tizen Studio (para desarrollo de app TV)

### Instalación con Docker

1. **Clona el repositorio**
```bash
git clone https://github.com/franeldramatico/HomeVision-Orchestrator.git
cd HomeVision-Orchestrator
```

2. **Configura las variables de entorno**
```bash
cp .env.example .env
# Edita .env con tu configuración (IP de TV Samsung, contraseñas, etc.)
```

3. **Levanta todos los servicios**
```bash
docker-compose up -d
```

4. **Accede a las interfaces**
- **API Gateway**: http://localhost:5000
- **Web Dashboard**: http://localhost:3000
- **Grafana Monitoring**: http://localhost:3001 (admin/admin)

### Configuración de TV Samsung

1. Habilita el modo desarrollador en tu TV Samsung
2. Instala Tizen Studio en tu PC
3. Empaqueta y despliega la app TV:
```bash
cd frontend/tizen-tv-app
npm install
npm run build
tizen package -t wgt -s your-certificate
tizen install -n HomeVision.wgt -t your-tv-ip
```

## 🛠️ Tecnologías

### Backend
- **ASP.NET Core 8.0** - API Gateway, Auth, Orchestration
- **Node.js** - Samsung TV integration service
- **SQL Server** - Base de datos principal
- **Redis** - Caching y pub/sub
- **SignalR/WebSockets** - Comunicación en tiempo real

### Frontend
- **Tizen (HTML5/JS)** - App nativa para Samsung TV
- **React + Vite** - Web SPA
- **React Native** - App móvil (opcional)
- **TailwindCSS** - Estilos modernos

### DevOps
- **Docker** - Containerización
- **Prometheus** - Métricas
- **Grafana** - Visualización

## 📂 Estructura del Proyecto

```
HomeVision-Orchestrator/
├── backend/                 # Microservicios ASP.NET Core + Node.js
│   ├── api-gateway/        # Gateway principal
│   ├── auth-service/       # Autenticación JWT
│   ├── orchestration-service/  # Motor de reglas
│   ├── tv-integration-service/ # Samsung TV API
│   ├── database/           # Scripts SQL
│   └── shared/             # Librerías comunes
├── frontend/
│   ├── tizen-tv-app/       # App Samsung TV
│   ├── web-spa/            # Dashboard web
│   └── mobile-app/         # App móvil
├── infra/                  # Infraestructura
│   ├── docker-compose.yml
│   ├── k8s/               # Kubernetes configs
│   └── monitoring/        # Prometheus/Grafana
└── docs/                  # Documentación
```

## 🔌 Integraciones Soportadas

- ✅ Luces inteligentes (Philips Hue, LIFX)
- ✅ Termostatos (Nest, Ecobee)
- ✅ Cámaras IP (RTSP/ONVIF)
- ✅ Enchufes inteligentes (TP-Link, Sonoff)
- ✅ Cerraduras inteligentes
- 🚧 Más en desarrollo...

## 🎯 Roadmap

- [x] Estructura base del proyecto
- [x] Backend microservicios
- [x] App Tizen funcional
- [x] Integración Samsung TV API
- [ ] Dashboard web completo
- [ ] Integración con dispositivos smart reales
- [ ] App móvil
- [ ] IA para automatizaciones predictivas

## 📄 Licencia

MIT License - ver archivo [LICENSE](LICENSE)

## 🤝 Contribuciones

Las contribuciones son bienvenidas! Por favor abre un issue o pull request.

## 📧 Contacto

- **Autor**: Frank Bramel
- **GitHub**: [@franeldramatico](https://github.com/franeldramatico)

---

⭐ Si este proyecto te es útil, ¡dale una estrella en GitHub!
