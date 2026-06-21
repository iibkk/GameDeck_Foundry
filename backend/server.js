import express from 'express';
import cors from 'cors';
import dotenv from 'dotenv';
import http from 'http';
import { WebSocketServer } from 'ws';

import authRoutes from './routes/auth.js';
import sessionRoutes from './routes/session.js';
import cardRoutes from './routes/card.js';
import gameRoutes from './routes/game.js';
import roomRoutes from './routes/room.js';

dotenv.config();

const app = express();

app.use(cors());
app.use(express.json());

app.use('/api/auth', authRoutes);
app.use('/api/session', sessionRoutes);
app.use('/api/cards', cardRoutes);
app.use('/api/game', gameRoutes);
app.use('/api/room', roomRoutes);

app.get('/', (req, res) => {
  res.send('GameDeck Backend Running');
});

const server = http.createServer(app);

const wss = new WebSocketServer({ server });

const rooms = new Map();

wss.on('connection', (ws) => {
  console.log('Client connected');

  ws.on('message', (message) => {
    const data = JSON.parse(message.toString());

    console.log('WS Message:', data);

    // JOIN ROOM
    if (data.type === 'join_room') {
      ws.session_id = data.session_id;

      if (!rooms.has(data.session_id)) {
        rooms.set(data.session_id, new Set());
      }

      rooms.get(data.session_id).add(ws);

      broadcast(data.session_id, {
        type: 'announcement',
        message: `${data.player_name} joined the game`
      });
    }

    if (data.type === 'play_card') {
      broadcast(data.session_id, {
        type: 'play_card',
        player_name: data.player_name,
        card_name: data.card_name,
        card_text: data.card_text,
        client_id: data.client_id,
        card_id: data.card_id,
        pile_id: data.pile_id,
        front_image_url: data.front_image_url,
        back_image_url: data.back_image_url,
        face_up: data.face_up
      });
    }

    // REMOVE PILE
    if (data.type === "remove_pile") {
      broadcast(data.session_id, {
        type: "remove_pile",
        client_id: data.client_id,
        pile_id: data.pile_id
      });
    }

    // WITHDRAW CARD
    if (data.type === 'withdraw_card') {
      broadcast(data.session_id, {
        type: 'withdraw_card',
        player_name: data.player_name,
        card_name: data.card_name,
        client_id: data.client_id,
        card_id: data.card_id
      });
    }
    if (data.type === "create_pile") {
      console.log("Broadcast create_pile:", data);

      broadcast(data.session_id, {
        type: "create_pile",
        client_id: data.client_id,
        pile_id: data.pile_id,
        x: data.x,
        y: data.y
      });
    }
    if (data.type === "move_pile") {
      console.log("Broadcast move_pile:", data);

      broadcast(data.session_id, {
        type: "move_pile",
        client_id: data.client_id,
        pile_id: data.pile_id,
        x: data.x,
        y: data.y
      });
    }
    //flip card
    if (data.type === "flip_cards") {
    broadcast(data.session_id, {
        type: "flip_cards",
        face_up: data.face_up
    });
    }
  });

  ws.on('close', () => {
    console.log('Client disconnected');

    if (ws.session_id && rooms.has(ws.session_id)) {
      rooms.get(ws.session_id).delete(ws);
    }
  });
});


function broadcast(sessionId, data) {
  const clients = rooms.get(sessionId);

  if (!clients) return;

  clients.forEach((client) => {
    if (client.readyState === 1) {
      client.send(JSON.stringify(data));
    }
  });
}

const PORT = process.env.PORT || 3000;

server.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});