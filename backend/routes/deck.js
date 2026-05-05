import express from 'express';

const router = express.Router();

router.post('/', async (req, res) => {
  console.log('Deck received:', req.body);

  res.json({
    message: 'Deck route working',
    deck: req.body
  });
});

export default router;