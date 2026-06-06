# PostgreSQL & pgAdmin 4 Setup Guide

Tài liệu hướng dẫn tạo Database và import schema cho hệ thống **Tour Guide System** bằng PostgreSQL và pgAdmin 4.

---

# 1. Yêu cầu hệ thống

Cài đặt các phần mềm sau:

* PostgreSQL 16 hoặc mới hơn
* pgAdmin 4
* File schema SQL của dự án

Kiểm tra PostgreSQL đã được cài đặt:

```bash
psql --version
```

Ví dụ:

```bash
psql (PostgreSQL) 16.4
```

---

# 2. Kết nối PostgreSQL Server trong pgAdmin 4

## Bước 1: Mở pgAdmin 4

Khởi động ứng dụng pgAdmin 4.

---

## Bước 2: Đăng ký Server

Trong thanh điều hướng bên trái:

```text
Servers
```

Nhấn chuột phải:

```text
Servers
 └── Register
      └── Server...
```

---

## Bước 3: Cấu hình Server

### Tab General

| Thuộc tính | Giá trị          |
| ---------- | ---------------- |
| Name       | Local PostgreSQL |

---

### Tab Connection

| Thuộc tính           | Giá trị             |
| -------------------- | ------------------- |
| Host name/address    | localhost           |
| Port                 | 5432                |
| Maintenance database | postgres            |
| Username             | postgres            |
| Password             | Mật khẩu PostgreSQL |

Đánh dấu:

```text
Save Password
```

Nhấn:

```text
Save
```

---

# 3. Tạo Database mới

Mở cây thư mục:

```text
Servers
 └── Local PostgreSQL
      └── Databases
```

Nhấn chuột phải:

```text
Databases
 └── Create
      └── Database...
```

---

## Tab General

| Thuộc tính | Giá trị      |
| ---------- | ------------ |
| Database   | tourguide_db |
| Owner      | postgres     |

Nhấn:

```text
Save
```

---

# 4. Kích hoạt Extension UUID

Mở Query Tool:

```text
tourguide_db
 └── Query Tool
```

Chạy:

```sql
CREATE EXTENSION IF NOT EXISTS pgcrypto;
```

Nhấn:

```text
Execute (F5)
```

Nếu thành công:

```text
CREATE EXTENSION
```

---

# 5. Kiểm tra PostgreSQL hỗ trợ UUID

Chạy:

```sql
SELECT gen_random_uuid();
```

Ví dụ:

```text
5b0eec76-8db2-48f3-ae0c-1e09f13af58f
```

Nếu có kết quả trả về nghĩa là UUID đã hoạt động.

---

# 6. Import Database Schema

## Cách 1: Import từ file SQL

Mở:

```text
Query Tool
```

Chọn biểu tượng:

```text
Open File
```

Chọn:

```text
database.sql
```

Nhấn:

```text
Execute (F5)
```

---

## Cách 2: Dán trực tiếp SQL

Mở:

```text
Query Tool
```

Dán toàn bộ nội dung schema.

Nhấn:

```text
Execute (F5)
```

---

# 7. Kiểm tra Tables

Refresh Database:

```text
Schemas
 └── public
      └── Tables
```

Các bảng mong đợi:

```text
languages

users
refresh_tokens

roles
admin_users

pois
poi_translations
poi_images

audios
qr_codes

tours
tour_pois

user_favorites

listening_sessions

user_location_logs

geofence_events

notifications
user_notifications

audit_logs
```

---

# 8. Seed dữ liệu ban đầu

## Languages

```sql
INSERT INTO languages(code, name)
VALUES
('vi','Tiếng Việt'),
('en','English'),
('zh','中文'),
('ja','日本語'),
('ko','한국어');
```

---

## Roles

```sql
INSERT INTO roles(name)
VALUES
('Admin'),
('Manager'),
('Editor');
```

---

# 9. Kiểm tra dữ liệu

Kiểm tra ngôn ngữ:

```sql
SELECT * FROM languages;
```

Kiểm tra role:

```sql
SELECT * FROM roles;
```

---

# 10. Backup Database

Trong pgAdmin:

```text
tourguide_db
 └── Backup...
```

Chọn:

```text
Format: Custom
```

Lưu:

```text
tourguide_db.backup
```

---

# 11. Restore Database

Tạo database mới:

```text
tourguide_db_restore
```

Nhấn:

```text
Restore...
```

Chọn file:

```text
tourguide_db.backup
```

Nhấn:

```text
Restore
```

---

# 12. Các lỗi thường gặp

## Lỗi

```text
function gen_random_uuid() does not exist
```

Khắc phục:

```sql
CREATE EXTENSION IF NOT EXISTS pgcrypto;
```

---

## Lỗi

```text
permission denied to create extension
```

Nguyên nhân:

User không có quyền Superuser.

Khắc phục:

Đăng nhập bằng tài khoản postgres.

---

## Lỗi

```text
relation already exists
```

Khắc phục:

Tạo database mới hoặc xóa bảng cũ.

---

## Lỗi

```text
syntax error near "Table"
```

Nguyên nhân:

Đang chạy file DrawSQL/DBML trong PostgreSQL.

Ví dụ sai:

```text
Table users {
}
```

PostgreSQL chỉ chấp nhận:

```sql
CREATE TABLE users (
);
```

---

# 13. Kiểm tra phiên bản PostgreSQL

```sql
SELECT version();
```

Ví dụ:

```text
PostgreSQL 16.4
```

---

# 14. Cấu trúc Database cuối cùng

```text
users
 ├── refresh_tokens
 ├── user_favorites
 ├── listening_sessions
 ├── user_location_logs
 ├── geofence_events
 └── user_notifications

pois
 ├── poi_translations
 ├── poi_images
 ├── audios
 ├── qr_codes
 ├── tour_pois
 ├── listening_sessions
 └── geofence_events

tours
 └── tour_pois

languages
 ├── poi_translations
 ├── audios
 └── listening_sessions

admin_users
 └── audit_logs
```

Sau khi hoàn thành các bước trên, cơ sở dữ liệu PostgreSQL cho hệ thống Tour Guide đã sẵn sàng để kết nối với ASP.NET Core Web API, Admin CMS và ứng dụng .NET MAUI.
