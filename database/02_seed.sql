-- GameDeck Foundry Sample Seed Data

-- 1. users
INSERT INTO users (full_name, email, password_hash, role)
VALUES
('Alice Teacher', 'alice.teacher@gamedeck.edu', 'hashed_password_123', 'teacher'),
('Bob Admin', 'bob.admin@gamedeck.edu', 'hashed_password_456', 'admin');

-- 2. games
INSERT INTO games (teacher_id, name, description, ruleset_type, visibility)
VALUES
(1, 'Cyber Safety Challenge', 'A classroom card game for learning cyber safety concepts.', 'quiz', 'classroom');

-- 3. decks
INSERT INTO decks (game_id, name, description)
VALUES
(1, 'Main Deck', 'Primary deck for the Cyber Safety Challenge game.');

-- 4. card_assets
INSERT INTO card_assets (game_id, front_image_url, back_image_url, template_name)
VALUES
(1, 'https://example.com/assets/front1.png', 'https://example.com/assets/back1.png', 'Standard Template');

-- 5. cards
INSERT INTO cards (deck_id, asset_id, card_name, card_type, position_in_deck)
VALUES
(1, 1, 'Question Card 1', 'question', 1),
(1, 1, 'Question Card 2', 'question', 2),
(1, 1, 'Scenario Card 1', 'scenario', 3);

-- 6. content_items
INSERT INTO content_items (game_id, title, body_text, topic, difficulty, tags)
VALUES
(1, 'Phishing Email', 'Identify the suspicious signs in this phishing email example.', 'Cybersecurity', 'easy', 'phishing,email,awareness'),
(1, 'Password Reuse', 'Explain why password reuse is risky in online systems.', 'Cybersecurity', 'medium', 'passwords,authentication'),
(1, 'Social Engineering Case', 'A student receives a fake urgent message asking for login credentials.', 'Cybersecurity', 'hard', 'social engineering,credentials');

-- 7. sessions
INSERT INTO sessions (game_id, join_code, status, started_at)
VALUES
(1, 'ABC123', 'active', CURRENT_TIMESTAMP);

-- 8. session_players
INSERT INTO session_players (session_id, player_name)
VALUES
(1, 'Student One'),
(1, 'Student Two');

-- 9. card_assignments
INSERT INTO card_assignments (session_id, card_id, content_id, assigned_to_player_id)
VALUES
(1, 1, 1, 1),
(1, 2, 2, 2),
(1, 3, 3, 1);

-- 10. event_logs
INSERT INTO event_logs (session_id, session_player_id, event_type, card_id, event_details)
VALUES
(1, 1, 'join', NULL, 'Student One joined the session'),
(1, 2, 'join', NULL, 'Student Two joined the session'),
(1, 1, 'draw', 1, 'Student One drew Question Card 1'),
(1, 2, 'draw', 2, 'Student Two drew Question Card 2'),
(1, 1, 'play', 3, 'Student One played Scenario Card 1');
