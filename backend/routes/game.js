import express from 'express';
import pool from '../db.js';

const router = express.Router();

router.post('/draw-card', async (req, res) => {
    try {
        const { deck_id, player_name } = req.body;

        const result = await pool.query(
            `SELECT * FROM cards 
       WHERE deck_id = $1 
       ORDER BY RANDOM() 
       LIMIT 1`,
            [deck_id]
        );

        if (result.rows.length === 0) {
            return res.status(404).json({ error: 'No cards in this deck' });
        }

        res.json({
            message: 'Card drawn',
            player_name,
            card: result.rows[0]
        });
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: 'Failed to draw card' });
    }
});

export default router;