import express from 'express';
import SamsungTvService from '../services/samsungTvService.js';

const router = express.Router();
const tvService = new SamsungTvService();

// Get TV status
router.get('/status', async (req, res) => {
    try {
        const status = await tvService.getStatus();
        res.json(status);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Power control
router.post('/power', async (req, res) => {
    try {
        const { powerOn } = req.body;
        await tvService.setPower(powerOn);
        res.json({ success: true, powerState: powerOn ? 'On' : 'Off' });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Volume control
router.post('/volume', async (req, res) => {
    try {
        const { level } = req.body;
        await tvService.setVolume(level);
        res.json({ success: true, volume: level });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Launch app
router.post('/launch-app', async (req, res) => {
    try {
        const { appId } = req.body;
        await tvService.launchApp(appId);
        res.json({ success: true, launchedApp: appId });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Display notification on TV
router.post('/display-notification', async (req, res) => {
    try {
        const { title, message, duration, type } = req.body;
        await tvService.displayNotification({ title, message, duration, type });
        res.json({ success: true, displayedAt: new Date().toISOString() });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Switch input
router.post('/input', async (req, res) => {
    try {
        const { inputSource } = req.body;
        await tvService.switchInput(inputSource);
        res.json({ success: true, currentInput: inputSource });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Send key command
router.post('/key', async (req, res) => {
    try {
        const { key } = req.body;
        await tvService.sendKey(key);
        res.json({ success: true, keySent: key });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

export default router;
