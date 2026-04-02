import express from 'express';
import pool from '../db.js';

const router = express.Router();

router.post('/join', async (req, res) => {
  const { player_name, join_code } = req.body;

  try {
    const sessionResult = await pool.query(
      'SELECT * FROM sessions WHERE join_code = $1',
      [join_code]
    );

    if (sessionResult.rows.length === 0) {
      return res.status(404).json({ message: 'Session not found' });
    }

    const session = sessionResult.rows[0];

    const playerResult = await pool.query(
      `INSERT INTO session_players (session_id, player_name)
       VALUES ($1, $2)
       RETURNING *`,
      [session.session_id, player_name]
    );

    const player = playerResult.rows[0];

    await pool.query(
      `INSERT INTO event_logs (session_id, session_player_id, event_type, event_details)
       VALUES ($1, $2, 'join', $3)`,
      [session.session_id, player.session_player_id, `${player_name} joined`]
    );

    res.json({ session, player });

  } catch (err) {
    console.error(err);
    res.status(500).json({ error: 'Server error' });
  }
});

export default router;
