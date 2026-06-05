# VERSA Admin CMS

Hệ thống quản trị nội dung (CMS) dành cho ứng dụng thuyết minh tự động theo vị trí GPS.

---

# Mục lục

- [1. Giới thiệu](#1-giới-thiệu)
- [2. Chức năng chính](#2-chức-năng-chính)
- [3. Công nghệ sử dụng](#3-công-nghệ-sử-dụng)
- [4. Các thư viện sử dụng](#4-các-thư-viện-sử-dụng)
- [5. Yêu cầu môi trường](#5-yêu-cầu-môi-trường)
- [6. Chạy dự án](#6-chạy-dự-án)

---

# 1. Giới thiệu

VERSA Admin CMS là hệ thống quản trị dữ liệu phục vụ ứng dụng thuyết minh tự động dựa trên GPS.

Hệ thống cho phép quản lý:

- Điểm thuyết minh (POI)
- Nội dung audio
- Bản dịch đa ngôn ngữ
- Tour tham quan
- QR Code
- Người dùng
- Lịch sử sử dụng
- Dashboard thống kê

---

# 2. Chức năng chính

## Dashboard

- Tổng số POI
- Tổng số Audio
- Tổng số Tour
- Tổng lượt sử dụng
- Thống kê truy cập

## Quản lý POI

- Thêm POI
- Sửa POI
- Xóa POI
- Quản lý tọa độ GPS
- Thiết lập bán kính Geofence
- Thiết lập mức ưu tiên

## Quản lý Audio

- Upload file MP3
- Nghe thử trực tiếp
- Thay thế và xóa file

## Quản lý Bản dịch

- Quản lý nội dung đa ngôn ngữ
- Liên kết với POI

## Quản lý Tour

- Tạo tour
- Chọn nhiều POI
- Sắp xếp thứ tự điểm tham quan

## Quản lý QR Code

- Sinh QR Code cho POI
- Sinh QR Code cho Tour
- Tải QR Code

## Quản lý Người dùng

- Đăng nhập
- Phân quyền
- Quản lý tài khoản

## Analytics

- Top POI được nghe nhiều nhất
- Thời gian nghe trung bình
- Thống kê theo ngày

---

# 3. Công nghệ sử dụng

## Backend MVC

- ASP.NET Core MVC (.NET 10)
- Razor View

## Database

- SQLite - Entity Framework Core (EF Core)

## Authentication

- ASP.NET Core Identity

## Frontend

- Bootstrap 5
- JavaScript

## Map

- LeafletJS
- OpenStreetMap

## QR Code

- QRCoder

## IDE

- Visual Studio Code

---

# 4. Các thư viện sử dụng

| Thư viện                                          | Chức năng                                             |
| ------------------------------------------------- | ----------------------------------------------------- |
| Microsoft.EntityFrameworkCore                     | ORM hỗ trợ thao tác dữ liệu bằng C# thay vì SQL thuần |
| Microsoft.EntityFrameworkCore.Sqlite              | Kết nối và làm việc với cơ sở dữ liệu SQLite          |
| Microsoft.EntityFrameworkCore.Design              | Hỗ trợ tạo và quản lý Migration                       |
| Microsoft.EntityFrameworkCore.Tools               | Cung cấp các lệnh `dotnet ef` để quản lý Database     |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | Hỗ trợ xác thực, đăng nhập và phân quyền người dùng   |
| QRCoder                                           | Tạo và xuất mã QR Code cho POI và Tour                |

---

# 5. Yêu cầu môi trường

## .NET SDK

Kiểm tra:

```bash
dotnet --version
```

Nếu chưa cài đặt:

https://dotnet.microsoft.com/download

---

## Cài đặt Extensions cho Visual Studio Code

Vào VS Code -> Extensions (Ctrl + Shift + X) -> Tìm kiếm và cài đặt

- C# Dev Kit
- C#
- NuGet Gallery
- SQLite Viewer

---

# 6. Chạy dự án

## Lần đầu clone dự án

Clone source code:

```bash
git clone <repository-url>
```

Di chuyển vào thư mục dự án:

```bash
cd AdminWeb
```

Khôi phục các package NuGet:

```bash
dotnet restore
```

Chạy ứng dụng:

```bash
dotnet run
```

Tìm dòng Now listening on: "URL" và mở "URL" trên web
