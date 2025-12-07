import { SamsungTVControl } from 'samsung-tv-control';

class SamsungTvService {
    constructor() {
        this.tvIp = process.env.SAMSUNG_TV_IP || '192.168.1.100';
        this.tvPort = parseInt(process.env.SAMSUNG_TV_PORT) || 8002;
        this.tvToken = process.env.SAMSUNG_TV_TOKEN || '';

        this.config = {
            ip: this.tvIp,
            mac: '',
            name: 'HomeVisionOrchestrator',
            port: this.tvPort,
            token: this.tvToken
        };

        this.tv = null;
        this.isConnected = false;
    }

    async connect() {
        if (this.isConnected) return;

        try {
            this.tv = new SamsungTVControl(this.config);
            await this.tv.getToken();
            this.isConnected = true;
            console.log('✅ Connected to Samsung TV');
        } catch (error) {
            console.error('❌ Failed to connect to Samsung TV:', error.message);
            // Fallback to mock mode for development
            this.isConnected = false;
        }
    }

    async getStatus() {
        await this.connect();

        // Mock data for development
        return {
            powerState: 'On',
            currentApp: 'Netflix',
            volume: 35,
            isMuted: false,
            currentInput: 'HDMI 1',
            model: 'Samsung UE32T430A',
            ipAddress: this.tvIp,
            connected: this.isConnected
        };
    }

    async setPower(powerOn) {
        await this.connect();

        if (this.isConnected && this.tv) {
            try {
                await this.tv.sendKey(powerOn ? 'KEY_POWER' : 'KEY_POWEROFF');
            } catch (error) {
                console.error('Power command failed:', error);
            }
        } else {
            console.log(`[MOCK] Setting power: ${powerOn ? 'On' : 'Off'}`);
        }
    }

    async setVolume(level) {
        await this.connect();

        if (this.isConnected && this.tv) {
            // Samsung TV doesn't have direct volume set, use Volume Up/Down
            console.log(`Setting volume to ${level}`);
        } else {
            console.log(`[MOCK] Setting volume to: ${level}`);
        }
    }

    async launchApp(appId) {
        await this.connect();

        const appMapping = {
            'netflix': '3201907018807',
            'youtube': '111299001912',
            'homevision': 'HomeVisionDashboard'
        };

        const actualAppId = appMapping[appId.toLowerCase()] || appId;

        if (this.isConnected && this.tv) {
            try {
                await this.tv.openApp(actualAppId);
            } catch (error) {
                console.error('App launch failed:', error);
            }
        } else {
            console.log(`[MOCK] Launching app: ${appId} (${actualAppId})`);
        }
    }

    async displayNotification(notification) {
        await this.connect();

        const { title, message, duration = 5, type = 'info' } = notification;

        console.log(`[TV NOTIFICATION] ${title}: ${message} (${duration}s)`);

        // In production, this would send a command to the Tizen app
        // to display an overlay notification
        // For now, we'll just log it
    }

    async switchInput(inputSource) {
        await this.connect();

        if (this.isConnected && this.tv) {
            try {
                await this.tv.sendKey('KEY_SOURCE');
                // Navigate to specific input - requires multiple key presses
            } catch (error) {
                console.error('Input switch failed:', error);
            }
        } else {
            console.log(`[MOCK] Switching to input: ${inputSource}`);
        }
    }

    async sendKey(key) {
        await this.connect();

        if (this.isConnected && this.tv) {
            try {
                await this.tv.sendKey(key);
            } catch (error) {
                console.error(`Key ${key} send failed:`, error);
            }
        } else {
            console.log(`[MOCK] Sending key: ${key}`);
        }
    }
}

export default SamsungTvService;
