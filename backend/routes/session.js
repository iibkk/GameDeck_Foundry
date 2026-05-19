import express from 'express';
import pool from '../db.js';
import { randomInt } from 'crypto';

const router = express.Router();

function generateJoinCode(length = 6) {
  const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
  let code = '';

  for (let i = 0; i < length; i++) {
    code += chars[randomInt(chars.length)];
  }

  return code;
}

// CREATE ROOM
router.post('/create', async (req, res) => {
  const { teacher_name } = req.body;

  try {
    let join_code;
    let exists = true;

    // make sure join_code is unique
    while (exists) {
      join_code = generateJoinCode();

      const checkResult = await pool.query(
        'SELECT * FROM sessions WHERE join_code = $1',
        [join_code]
      );

      exists = checkResult.rows.length > 0;
    }

    const sessionResult = await pool.query(
      `INSERT INTO sessions (join_code, teacher_name)
       VALUES ($1, $2)
       RETURNING *`,
      [join_code, teacher_name || 'Teacher']
    );

    const session = sessionResult.rows[0];

    res.json({
      success: true,
      session,
      roomCode: session.join_code,
      join_code: session.join_code,
      session_id: session.session_id
    });

  } catch (err) {
    console.error('Create session error:', err);
    res.status(500).json({ error: 'Failed to create session' });
  }
});

// JOIN ROOM
router.post('/join', async (req, res) => {
  const { player_name, join_code } = req.body;

  try {
    const sessionResult = await pool.query(
      'SELECT * FROM sessions WHERE join_code = $1',
      [join_code]
    );

    if (sessionResult.rows.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Session not found'
      });
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

    res.json({
      success: true,
      session,
      player,
      roomCode: session.join_code,
      join_code: session.join_code,
      session_id: session.session_id
    });

  } catch (err) {
    console.error('Join session error:', err);
    res.status(500).json({ error: 'Server error' });
  }
});

export default router;