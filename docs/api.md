# HomeVision Orchestrator - API Documentation

## Base URLs

- **API Gateway**: `http://localhost:5000/api`
- **Auth Service**: `http://localhost:5001/api`
- **TV Integration**: `http://localhost:3000/api/tv`
- **WebSocket**: `ws://localhost:8080`

## Authentication

All API endpoints (except `/api/auth/*`) require a JWT Bearer token.

```bash
# Get token
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Use token
curl http://localhost:5000/api/devices \
  -H "Authorization: Bearer YOUR_TOKEN"
```

## API Endpoints

### Devices

#### GET `/api/devices`
List all devices.

**Response:**
```json
[
  {
    "Id": 1,
    "Name": "Living Room Light",
    "Type": "Light",
    "Status": "On",
    "Room": "Living Room"
  }
]
```

#### POST `/api/devices/{id}/control`
Control a device.

**Request:**
```json
{
  "Action": "toggle",
  "Parameters": {
    "brightness": 80
  }
}
```

### Scenes

#### GET `/api/scenes`
List all automation scenes.

#### POST `/api/scenes/{id}/execute`
Execute a scene.

### Notifications

#### GET `/api/notifications`
Get recent notifications.

#### POST `/api/notifications`
Create a new notification (will display on TV).

**Request:**
```json
{
  "Type": "security",
  "Title": "Puerta Abierta",
  "Message": "La puerta frontal fue abierta",
  "Priority": "high",
  "SendToTv": true
}
```

### Samsung TV Control

#### GET `/api/tv/status`
Get TV status.

#### POST `/api/tv/power`
Turn TV on/off.

**Request:**
```json
{
  "PowerOn": true
}
```

#### POST `/api/tv/launch-app`
Launch an app on the TV.

**Request:**
```json
{
  "AppId": "netflix"
}
```

## WebSocket Events

Connect to `ws://localhost:8080` for real-time updates.

**Server → Client:**
```json
{
  "type": "notification",
  "icon": "🚪",
  "title": "Door Opened",
  "message": "Front door was opened"
}
```

**Client → Server:**
```json
{
  "type": "tv-command",
  "command": "launchApp",
  "params": {"appId": "netflix"}
}
```
