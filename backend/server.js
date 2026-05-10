import express from 'express';
import cors from 'cors';
import dotenv from 'dotenv';

import authRoutes from './routes/auth.js';
import sessionRoutes from './routes/session.js';
import cardRoutes from './routes/card.js';
import deckRoutes from './routes/deck.js';
import gameRoutes from './routes/game.js';

dotenv.config();

const app = express();

app.use(cors());
app.use(express.json());

app.use('/api/auth', authRoutes);
app.use('/api/session', sessionRoutes);
app.use('/api/cards', cardRoutes);
app.use('/api/decks', deckRoutes);
app.use('/api/game', gameRoutes);

app.get('/', (req, res) => {
  res.send('GameDeck Backend Running');
});

const PORT = process.env.PORT || 3000;

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});