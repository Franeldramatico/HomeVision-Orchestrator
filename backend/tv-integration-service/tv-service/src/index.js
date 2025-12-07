import express from 'express';
import cors from 'cors';
import dotenv from 'dotenv';
import { WebSocketServer } from 'ws';
import tvController from './controllers/tvController.js';

dotenv.config();

const app = express();
const PORT = process.env.PORT || 3000;
const WS_PORT = process.env.WS_PORT || 8080;

// Middleware
app.use(cors());
app.use(express.json());

// Routes
app.use('/api/tv', tvController);

// Health check
app.get('/health', (req, res) => {
    res.json({ status: 'healthy', service: 'tv-integration', timestamp: new Date().toISOString() });
});

// HTTP Server
app.listen(PORT, () => {
    console.log(`🖥️  TV Integration Service running on port ${PORT}`);
    console.log(`📺 Samsung TV IP: ${process.env.SAMSUNG_TV_IP || 'Not configured'}`);
});

// WebSocket Server for real-time TV control
const wss = new WebSocketServer({ port: WS_PORT });

wss.on('connection', (ws) => {
    console.log('📡 New WebSocket client connected');

    ws.on('message', (message) => {
        try {
            const data = JSON.parse(message);
            console.log('Received WebSocket message:', data);

            // Handle different message types
            switch (data.type) {
                case 'tv-command':
                    // Forward command to Samsung TV
                    handleTvCommand(data.command, data.params);
                    ws.send(JSON.stringify({ type: 'ack', status: 'success' }));
                    break;

                case 'ping':
                    ws.send(JSON.stringify({ type: 'pong', timestamp: Date.now() }));
                    break;

                default:
                    ws.send(JSON.stringify({ type: 'error', message: 'Unknown message type' }));
            }
        } catch (error) {
            console.error('WebSocket message error:', error);
            ws.send(JSON.stringify({ type: 'error', message: error.message }));
        }
    });

    ws.on('close', () => {
        console.log('📴 WebSocket client disconnected');
    });

    // Send welcome message
    ws.send(JSON.stringify({
        type: 'connected',
        message: 'Connected to HomeVision TV Service',
        timestamp: Date.now()
    }));
});

console.log(`🔌 WebSocket server running on port ${WS_PORT}`);

// TV command handler
function handleTvCommand(command, params) {
    // This will be implemented with actual Samsung TV SDK
    console.log(`Executing TV command: ${command}`, params);
}

// Broadcast to all connected clients
export function broadcastToClients(data) {
    wss.clients.forEach((client) => {
        if (client.readyState === 1) { // WebSocket.OPEN
            client.send(JSON.stringify(data));
        }
    });
}
