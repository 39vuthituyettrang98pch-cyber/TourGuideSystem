CREATE TABLE `languages` (
  `id` uuid PRIMARY KEY,
  `code` varchar(10) UNIQUE NOT NULL,
  `name` varchar(100) NOT NULL
);

CREATE TABLE `roles` (
  `id` uuid PRIMARY KEY,
  `code` varchar(50) UNIQUE NOT NULL,
  `name` varchar(100) NOT NULL
);

CREATE TABLE `user_profiles` (
  `user_id` uuid PRIMARY KEY,
  `avatar_url` text,
  `preferred_language_id` uuid,
  `created_at` timestamp NOT NULL
);

CREATE TABLE `users` (
  `id` uuid PRIMARY KEY,
  `role_id` uuid NOT NULL,
  `email` varchar(255) UNIQUE NOT NULL,
  `password_hash` text NOT NULL,
  `full_name` varchar(255),
  `is_active` boolean NOT NULL DEFAULT true,
  `created_at` timestamp NOT NULL,
  `updated_at` timestamp
);

CREATE TABLE `pois` (
  `id` uuid PRIMARY KEY,
  `code` varchar(50) UNIQUE,
  `latitude` decimal(18,15) NOT NULL,
  `longitude` decimal(18,15) NOT NULL,
  `trigger_radius` float,
  `priority` int NOT NULL DEFAULT 1,
  `thumbnail_url` text,
  `map_url` text,
  `is_active` boolean NOT NULL DEFAULT true,
  `created_at` timestamp NOT NULL,
  `updated_at` timestamp
);

CREATE TABLE `poi_translations` (
  `id` uuid PRIMARY KEY,
  `poi_id` uuid NOT NULL,
  `language_id` uuid NOT NULL,
  `name` varchar(255) NOT NULL,
  `short_description` text,
  `full_description` text,
  `created_at` timestamp NOT NULL
);

CREATE TABLE `poi_images` (
  `id` uuid PRIMARY KEY,
  `poi_id` uuid NOT NULL,
  `image_url` text NOT NULL,
  `sort_order` int NOT NULL DEFAULT 0,
  `created_at` timestamp NOT NULL
);

CREATE TABLE `audios` (
  `id` uuid PRIMARY KEY,
  `poi_id` uuid NOT NULL,
  `language_id` uuid NOT NULL,
  `title` varchar(255),
  `audio_url` text NOT NULL,
  `duration_seconds` int,
  `is_generated_by_tts` boolean NOT NULL DEFAULT false,
  `tts_voice` varchar(100),
  `created_at` timestamp NOT NULL
);

CREATE TABLE `qr_codes` (
  `id` uuid PRIMARY KEY,
  `poi_id` uuid NOT NULL,
  `qr_value` varchar(255) UNIQUE NOT NULL,
  `created_at` timestamp NOT NULL
);

CREATE TABLE `tours` (
  `id` uuid PRIMARY KEY,
  `name` varchar(255) NOT NULL,
  `thumbnail_url` text,
  `description` text,
  `estimated_duration` int,
  `is_active` boolean NOT NULL DEFAULT true,
  `created_at` timestamp NOT NULL
);

CREATE TABLE `tour_pois` (
  `id` uuid PRIMARY KEY,
  `tour_id` uuid NOT NULL,
  `poi_id` uuid NOT NULL,
  `sort_order` int NOT NULL
);

CREATE TABLE `user_favorites` (
  `user_id` uuid NOT NULL,
  `poi_id` uuid NOT NULL,
  `created_at` timestamp NOT NULL,
  PRIMARY KEY (`user_id`, `poi_id`)
);

CREATE TABLE `listening_sessions` (
  `id` uuid PRIMARY KEY,
  `user_id` uuid NOT NULL,
  `poi_id` uuid NOT NULL,
  `audio_id` uuid,
  `language_id` uuid,
  `source` varchar(50),
  `start_time` timestamp NOT NULL,
  `end_time` timestamp,
  `duration_seconds` int,
  `created_at` timestamp NOT NULL
);

CREATE TABLE `user_location_logs` (
  `id` uuid PRIMARY KEY,
  `user_id` uuid NOT NULL,
  `latitude` decimal(18,15) NOT NULL,
  `longitude` decimal(18,15) NOT NULL,
  `recorded_at` timestamp NOT NULL
);

CREATE TABLE `geofence_events` (
  `id` uuid PRIMARY KEY,
  `user_id` uuid NOT NULL,
  `poi_id` uuid NOT NULL,
  `event_type` varchar(20) NOT NULL,
  `distance` float,
  `event_time` timestamp NOT NULL
);

CREATE TABLE `notifications` (
  `id` uuid PRIMARY KEY,
  `title` varchar(255) NOT NULL,
  `content` text NOT NULL,
  `created_at` timestamp NOT NULL
);

CREATE TABLE `user_notifications` (
  `user_id` uuid NOT NULL,
  `notification_id` uuid NOT NULL,
  `is_read` boolean NOT NULL DEFAULT false,
  `read_at` timestamp,
  PRIMARY KEY (`user_id`, `notification_id`)
);

CREATE UNIQUE INDEX `poi_translations_index_0` ON `poi_translations` (`poi_id`, `language_id`);

CREATE UNIQUE INDEX `audios_index_1` ON `audios` (`poi_id`, `language_id`);

ALTER TABLE `poi_translations` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `poi_translations` ADD FOREIGN KEY (`language_id`) REFERENCES `languages` (`id`);

ALTER TABLE `poi_images` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `audios` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `audios` ADD FOREIGN KEY (`language_id`) REFERENCES `languages` (`id`);

ALTER TABLE `qr_codes` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `tour_pois` ADD FOREIGN KEY (`tour_id`) REFERENCES `tours` (`id`);

ALTER TABLE `tour_pois` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `user_favorites` ADD FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

ALTER TABLE `user_favorites` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `listening_sessions` ADD FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

ALTER TABLE `listening_sessions` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `listening_sessions` ADD FOREIGN KEY (`audio_id`) REFERENCES `audios` (`id`);

ALTER TABLE `listening_sessions` ADD FOREIGN KEY (`language_id`) REFERENCES `languages` (`id`);

ALTER TABLE `user_location_logs` ADD FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

ALTER TABLE `geofence_events` ADD FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

ALTER TABLE `geofence_events` ADD FOREIGN KEY (`poi_id`) REFERENCES `pois` (`id`);

ALTER TABLE `user_notifications` ADD FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

ALTER TABLE `user_notifications` ADD FOREIGN KEY (`notification_id`) REFERENCES `notifications` (`id`);

ALTER TABLE `users` ADD FOREIGN KEY (`role_id`) REFERENCES `roles` (`id`);

ALTER TABLE `user_profiles` ADD FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

ALTER TABLE `user_profiles` ADD FOREIGN KEY (`preferred_language_id`) REFERENCES `languages` (`id`);
