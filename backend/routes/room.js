import express from 'express';

const router = express.Router();

function generateRoomCode() {
  return Math.random().toString(36).substring(2, 8).toUpperCase();
}

const rooms = new Set();

router.post('/create', (req, res) => {
  const roomCode = generateRoomCode();
  rooms.add(roomCode);

  res.json({
    success: true,
    roomCode
  });
});

router.post('/join', (req, res) => {
  const { roomCode } = req.body;

  if (!rooms.has(roomCode)) {
    return res.status(404).json({ error: 'Room not found' });
  }

  res.json({
    success: true,
    roomCode
  });
});

export default router;