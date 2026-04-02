-- GameDeck Foundry Database Schema
-- Recommended DBMS: PostgreSQL

-- Drop tables in reverse dependency order if needed
DROP TABLE IF EXISTS event_logs CASCADE;
DROP TABLE IF EXISTS card_assignments CASCADE;
DROP TABLE IF EXISTS session_players CASCADE;
DROP TABLE IF EXISTS sessions CASCADE;
DROP TABLE IF EXISTS content_items CASCADE;
DROP TABLE IF EXISTS cards CASCADE;
DROP TABLE IF EXISTS card_assets CASCADE;
DROP TABLE IF EXISTS decks CASCADE;
DROP TABLE IF EXISTS games CASCADE;
DROP TABLE IF EXISTS users CASCADE;

-- 1. users
CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    role VARCHAR(20) NOT NULL CHECK (role IN ('teacher', 'admin')),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 2. games
CREATE TABLE games (
    game_id SERIAL PRIMARY KEY,
    teacher_id INTEGER NOT NULL,
    name VARCHAR(150) NOT NULL,
    description TEXT,
    ruleset_type VARCHAR(50),
    visibility VARCHAR(20) DEFAULT 'private' CHECK (visibility IN ('private', 'public', 'classroom')),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_games_teacher
        FOREIGN KEY (teacher_id)
        REFERENCES users(user_id)
        ON DELETE CASCADE
);

-- 3. decks
CREATE TABLE decks (
    deck_id SERIAL PRIMARY KEY,
    game_id INTEGER NOT NULL,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_decks_game
        FOREIGN KEY (game_id)
        REFERENCES games(game_id)
        ON DELETE CASCADE
);

-- 4. card_assets
CREATE TABLE card_assets (
    asset_id SERIAL PRIMARY KEY,
    game_id INTEGER NOT NULL,
    front_image_url TEXT NOT NULL,
    back_image_url TEXT NOT NULL,
    template_name VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_card_assets_game
        FOREIGN KEY (game_id)
        REFERENCES games(game_id)
        ON DELETE CASCADE
);

-- 5. cards
CREATE TABLE cards (
    card_id SERIAL PRIMARY KEY,
    deck_id INTEGER NOT NULL,
    asset_id INTEGER,
    card_name VARCHAR(100) NOT NULL,
    card_type VARCHAR(50),
    position_in_deck INTEGER,
    CONSTRAINT fk_cards_deck
        FOREIGN KEY (deck_id)
        REFERENCES decks(deck_id)
        ON DELETE CASCADE,
    CONSTRAINT fk_cards_asset
        FOREIGN KEY (asset_id)
        REFERENCES card_assets(asset_id)
        ON DELETE SET NULL
);

-- 6. content_items
CREATE TABLE content_items (
    content_id SERIAL PRIMARY KEY,
    game_id INTEGER NOT NULL,
    title VARCHAR(150) NOT NULL,
    body_text TEXT NOT NULL,
    topic VARCHAR(100),
    difficulty VARCHAR(20) CHECK (difficulty IN ('easy', 'medium', 'hard')),
    tags TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_content_items_game
        FOREIGN KEY (game_id)
        REFERENCES games(game_id)
        ON DELETE CASCADE
);

-- 7. sessions
CREATE TABLE sessions (
    session_id SERIAL PRIMARY KEY,
    game_id INTEGER NOT NULL,
    teacher_id INTEGER,
    join_code VARCHAR(20) NOT NULL UNIQUE,
    status VARCHAR(20) DEFAULT 'pending' CHECK (status IN ('pending', 'active', 'paused', 'ended')),
    started_at TIMESTAMP,
    ended_at TIMESTAMP,
    CONSTRAINT fk_sessions_game
        FOREIGN KEY (game_id)
        REFERENCES games(game_id)
        ON DELETE CASCADE,
    CONSTRAINT fk_sessions_teacher
        FOREIGN KEY (teacher_id)
        REFERENCES users(user_id)
        ON DELETE SET NULL
);

-- 8. session_players
CREATE TABLE session_players (
    session_player_id SERIAL PRIMARY KEY,
    session_id INTEGER NOT NULL,
    player_name VARCHAR(100) NOT NULL,
    joined_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_session_players_session
        FOREIGN KEY (session_id)
        REFERENCES sessions(session_id)
        ON DELETE CASCADE
);

-- 9. card_assignments
CREATE TABLE card_assignments (
    assignment_id SERIAL PRIMARY KEY,
    session_id INTEGER NOT NULL,
    card_id INTEGER NOT NULL,
    content_id INTEGER NOT NULL,
    assigned_to_player_id INTEGER,
    assigned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_card_assignments_session
        FOREIGN KEY (session_id)
        REFERENCES sessions(session_id)
        ON DELETE CASCADE,
    CONSTRAINT fk_card_assignments_card
        FOREIGN KEY (card_id)
        REFERENCES cards(card_id)
        ON DELETE CASCADE,
    CONSTRAINT fk_card_assignments_content
        FOREIGN KEY (content_id)
        REFERENCES content_items(content_id)
        ON DELETE CASCADE,
    CONSTRAINT fk_card_assignments_player
        FOREIGN KEY (assigned_to_player_id)
        REFERENCES session_players(session_player_id)
        ON DELETE SET NULL
);

-- 10. event_logs
CREATE TABLE event_logs (
    event_id SERIAL PRIMARY KEY,
    session_id INTEGER NOT NULL,
    session_player_id INTEGER,
    event_type VARCHAR(50) NOT NULL CHECK (event_type IN ('draw', 'play', 'discard', 'flip', 'join', 'leave')),
    card_id INTEGER,
    event_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    event_details TEXT,
    CONSTRAINT fk_event_logs_session
        FOREIGN KEY (session_id)
        REFERENCES sessions(session_id)
        ON DELETE CASCADE,
    CONSTRAINT fk_event_logs_player
        FOREIGN KEY (session_player_id)
        REFERENCES session_players(session_player_id)
        ON DELETE SET NULL,
    CONSTRAINT fk_event_logs_card
        FOREIGN KEY (card_id)
        REFERENCES cards(card_id)
        ON DELETE SET NULL
);

-- Indexes for performance
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_games_teacher_id ON games(teacher_id);
CREATE INDEX idx_decks_game_id ON decks(game_id);
CREATE INDEX idx_cards_deck_id ON cards(deck_id);
CREATE INDEX idx_sessions_join_code ON sessions(join_code);
CREATE INDEX idx_session_players_session_id ON session_players(session_id);
CREATE INDEX idx_card_assignments_session_id ON card_assignments(session_id);
CREATE INDEX idx_event_logs_session_id ON event_logs(session_id);
