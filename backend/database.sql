-- =====================================================
-- TOUR GUIDE SYSTEM DATABASE
-- PostgreSQL
-- =====================================================

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- =====================================================
-- LANGUAGES
-- =====================================================

CREATE TABLE languages (
    id SERIAL PRIMARY KEY,
    code VARCHAR(10) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL
);

-- =====================================================
-- USERS
-- =====================================================

CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    full_name VARCHAR(255),
    avatar_url TEXT,
    preferred_language_id INT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,

    CONSTRAINT fk_users_language
        FOREIGN KEY (preferred_language_id)
        REFERENCES languages(id)
);

-- =====================================================
-- REFRESH TOKENS
-- =====================================================

CREATE TABLE refresh_tokens (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    user_id UUID NOT NULL,
    token TEXT NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    is_revoked BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_refresh_tokens_user
        FOREIGN KEY (user_id)
        REFERENCES users(id)
        ON DELETE CASCADE
);

-- =====================================================
-- ROLES
-- =====================================================

CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

-- =====================================================
-- ADMIN USERS
-- =====================================================

CREATE TABLE admin_users (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    role_id INT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    last_login_at TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_admin_role
        FOREIGN KEY (role_id)
        REFERENCES roles(id)
);

-- =====================================================
-- POI
-- =====================================================

CREATE TABLE pois (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    code VARCHAR(50) UNIQUE,
    latitude DECIMAL(18,15) NOT NULL,
    longitude DECIMAL(18,15) NOT NULL,
    trigger_radius DOUBLE PRECISION NOT NULL,
    priority INT NOT NULL DEFAULT 1,
    thumbnail_url TEXT,
    map_url TEXT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

-- =====================================================
-- POI TRANSLATIONS
-- =====================================================

CREATE TABLE poi_translations (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    poi_id UUID NOT NULL,
    language_id INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    short_description TEXT,
    full_description TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_translation_poi
        FOREIGN KEY (poi_id)
        REFERENCES pois(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_translation_language
        FOREIGN KEY (language_id)
        REFERENCES languages(id),

    CONSTRAINT uq_poi_language
        UNIQUE (poi_id, language_id)
);

-- =====================================================
-- POI IMAGES
-- =====================================================

CREATE TABLE poi_images (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    poi_id UUID NOT NULL,
    image_url TEXT NOT NULL,
    sort_order INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_poi_images
        FOREIGN KEY (poi_id)
        REFERENCES pois(id)
        ON DELETE CASCADE
);

-- =====================================================
-- AUDIOS
-- =====================================================

CREATE TABLE audios (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    poi_id UUID NOT NULL,
    language_id INT NOT NULL,
    title VARCHAR(255),
    audio_url TEXT NOT NULL,
    duration_seconds INT,
    is_generated_by_tts BOOLEAN NOT NULL DEFAULT FALSE,
    tts_voice VARCHAR(100),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_audio_poi
        FOREIGN KEY (poi_id)
        REFERENCES pois(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_audio_language
        FOREIGN KEY (language_id)
        REFERENCES languages(id),

    CONSTRAINT uq_audio_language
        UNIQUE (poi_id, language_id)
);

-- =====================================================
-- QR CODES
-- =====================================================

CREATE TABLE qr_codes (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    poi_id UUID NOT NULL,
    qr_value VARCHAR(255) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_qr_poi
        FOREIGN KEY (poi_id)
        REFERENCES pois(id)
        ON DELETE CASCADE
);

-- =====================================================
-- TOURS
-- =====================================================

CREATE TABLE tours (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    name VARCHAR(255) NOT NULL,
    thumbnail_url TEXT,
    description TEXT,
    estimated_duration INT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- TOUR POI
-- =====================================================

CREATE TABLE tour_pois (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    tour_id UUID NOT NULL,
    poi_id UUID NOT NULL,
    sort_order INT NOT NULL,

    CONSTRAINT fk_tourpoi_tour
        FOREIGN KEY (tour_id)
        REFERENCES tours(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_tourpoi_poi
        FOREIGN KEY (poi_id)
    REFERENCES pois(id)
    ON DELETE CASCADE
);

-- =====================================================
-- FAVORITES
-- =====================================================

CREATE TABLE user_favorites (
    user_id UUID NOT NULL,
    poi_id UUID NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (user_id, poi_id),

    CONSTRAINT fk_favorite_user
        FOREIGN KEY (user_id)
        REFERENCES users(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_favorite_poi
        FOREIGN KEY (poi_id)
        REFERENCES pois(id)
        ON DELETE CASCADE
);

-- =====================================================
-- LISTENING SESSIONS
-- =====================================================

CREATE TABLE listening_sessions (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    user_id UUID NOT NULL,
    poi_id UUID NOT NULL,
    audio_id UUID,
    language_id INT,
    source VARCHAR(50),
    start_time TIMESTAMP NOT NULL,
    end_time TIMESTAMP,
    duration_seconds INT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_session_user
        FOREIGN KEY (user_id)
        REFERENCES users(id),

    CONSTRAINT fk_session_poi
        FOREIGN KEY (poi_id)
        REFERENCES pois(id),

    CONSTRAINT fk_session_audio
        FOREIGN KEY (audio_id)
        REFERENCES audios(id),

    CONSTRAINT fk_session_language
        FOREIGN KEY (language_id)
        REFERENCES languages(id)
);

-- =====================================================
-- USER LOCATION LOGS
-- =====================================================

CREATE TABLE user_location_logs (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    user_id UUID NOT NULL,
    latitude DECIMAL(18,15) NOT NULL,
    longitude DECIMAL(18,15) NOT NULL,
    recorded_at TIMESTAMP NOT NULL,

    CONSTRAINT fk_location_user
        FOREIGN KEY (user_id)
        REFERENCES users(id)
        ON DELETE CASCADE
);

-- =====================================================
-- GEOFENCE EVENTS
-- =====================================================

CREATE TABLE geofence_events (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    user_id UUID NOT NULL,
    poi_id UUID NOT NULL,
    event_type VARCHAR(20) NOT NULL,
    distance DOUBLE PRECISION,
    event_time TIMESTAMP NOT NULL,
    CONSTRAINT fk_geofence_user
        FOREIGN KEY (user_id)
        REFERENCES users(id),

    CONSTRAINT fk_geofence_poi
        FOREIGN KEY (poi_id)
        REFERENCES pois(id)
);

-- =====================================================
-- NOTIFICATIONS
-- =====================================================

CREATE TABLE notifications (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    title VARCHAR(255) NOT NULL,
    content TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- USER NOTIFICATIONS
-- =====================================================

CREATE TABLE user_notifications (
    user_id UUID NOT NULL,
    notification_id UUID NOT NULL,
    is_read BOOLEAN NOT NULL DEFAULT FALSE,
    read_at TIMESTAMP,
    PRIMARY KEY (user_id, notification_id),

    CONSTRAINT fk_user_notification_user
        FOREIGN KEY (user_id)
        REFERENCES users(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_user_notification_notification
        FOREIGN KEY (notification_id)
        REFERENCES notifications(id)
        ON DELETE CASCADE
);

-- =====================================================
-- AUDIT LOGS
-- =====================================================

CREATE TABLE audit_logs (
    id UUID PRIMARY KEY DEFAULT uuidv7(),
    admin_id UUID NOT NULL,
    action VARCHAR(100) NOT NULL,
    entity_type VARCHAR(100) NOT NULL,
    entity_id UUID,
    old_data JSONB,
    new_data JSONB,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  
    CONSTRAINT fk_audit_admin
        FOREIGN KEY (admin_id)
        REFERENCES admin_users(id)
);

-- =====================================================
-- INDEXES
-- =====================================================

CREATE INDEX idx_poi_location
ON pois(latitude, longitude);

CREATE INDEX idx_location_user
ON user_location_logs(user_id);

CREATE INDEX idx_location_time
ON user_location_logs(recorded_at);

CREATE INDEX idx_listening_user
ON listening_sessions(user_id);

CREATE INDEX idx_listening_poi
ON listening_sessions(poi_id);

CREATE INDEX idx_geofence_user
ON geofence_events(user_id);

CREATE INDEX idx_geofence_poi
ON geofence_events(poi_id);

CREATE INDEX idx_audio_poi
ON audios(poi_id);

CREATE INDEX idx_translation_poi
ON poi_translations(poi_id);
