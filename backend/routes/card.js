import express from 'express';
import pool from '../db.js';

const router = express.Router();

// Add card to deck
router.post('/', async (req, res) => {
  try {
    const {
      deck_id,
      card_name,
      card_text,
      description,
      front_image_url,
      back_image_url
    } = req.body;

    const result = await pool.query(
      `INSERT INTO cards 
      (deck_id, card_name, card_text, description, front_image_url, back_image_url)
      VALUES ($1, $2, $3, $4, $5, $6)
      RETURNING *`,
      [deck_id, card_name, card_text, description, front_image_url, back_image_url]
    );

    res.json({
      message: 'Card added',
      card: result.rows[0]
    });
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: 'Failed to add card' });
  }
});

// Get cards in deck
router.get('/', async (req, res) => {
  try {
    const { deck_id } = req.query;

    const result = await pool.query(
      'SELECT * FROM cards WHERE deck_id = $1 ORDER BY id ASC',
      [deck_id]
    );

    res.json(result.rows);
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: 'Failed to get cards' });
  }
});

export default router;