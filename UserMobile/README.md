# VERSA Mobile App

Ứng dụng thuyết minh tự động theo vị trí GPS dành cho Android và iOS, được xây dựng bằng .NET MAUI.

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

VERSA Mobile là ứng dụng thuyết minh tự động dựa trên GPS.

Khi người dùng di chuyển đến gần một điểm tham quan (POI), hệ thống sẽ tự động phát nội dung thuyết minh bằng Audio hoặc Text To Speech (TTS).

Ứng dụng hỗ trợ:

- GPS Tracking thời gian thực
- Geofencing
- Tự động phát Audio/TTS
- Quét QR Code
- Làm việc Offline
- Đồng bộ dữ liệu từ Server

---

# 2. Chức năng chính

## GPS Tracking

- Theo dõi vị trí người dùng liên tục
- Hỗ trợ Foreground Service
- Hỗ trợ Background Tracking
- Tối ưu pin và độ chính xác

## Geofence

- Xác định POI gần nhất
- Kích hoạt theo bán kính
- Kích hoạt theo mức ưu tiên
- Chống kích hoạt lặp

## Audio Narration

- Phát Audio MP3
- Hỗ trợ Text To Speech (TTS)
- Hàng đợi phát nội dung
- Cooldown chống phát lặp

## Map View

- Hiển thị vị trí người dùng
- Hiển thị danh sách POI
- Highlight POI gần nhất
- Xem chi tiết POI

## QR Code

- Quét QR Code
- Mở trực tiếp nội dung POI
- Không cần GPS

## Offline Mode

- Lưu POI trên thiết bị
- Lưu Audio Metadata
- Đồng bộ khi có Internet

## Analytics

- Lưu lịch sử nghe
- Top POI được nghe nhiều nhất
- Thời gian nghe trung bình
- Heatmap vị trí người dùng

---

# 3. Công nghệ sử dụng

## Framework

- .NET MAUI (.NET 10)

## UI

- XAML
- MVVM

## GPS

- MAUI Geolocation API
- Android FusedLocationProvider
- iOS CLLocationManager

## Database

- SQLite

## Audio

- Plugin.Maui.Audio
- Text To Speech API

## Maps

- Microsoft.Maui.Controls.Maps

## QR Code

- ZXing.Net.Maui

## IDE

- Visual Studio Code

---

# 4. Các thư viện sử dụng

| Thư viện                     | Chức năng                        |
| ---------------------------- | -------------------------------- |
| sqlite-net-pcl               | Lưu dữ liệu SQLite trên thiết bị |
| CommunityToolkit.Maui        | Hỗ trợ MVVM và UI                |
| Plugin.Maui.Audio            | Phát file âm thanh               |
| ZXing.Net.Maui               | Quét QR Code                     |
| Microsoft.Maui.Controls.Maps | Hiển thị bản đồ                  |
| Microsoft.Extensions.Http    | Kết nối Web API                  |

---

# 5. Yêu cầu môi trường

## .NET SDK

Kiểm tra:

```bash
dotnet --version
```

Nếu chưa cài đặt, tải và cài đặt .NET 10 tại: https://dotnet.microsoft.com/download

---

## Android SDK

Kiểm tra:

```bash
adb version
```

Nếu chưa cài đặt:

- Tải và cài đặt SDK Platform-Tools cho Windows tại: https://developer.android.com/tools/releases/platform-tools
- Giải nén và chạy

---

## Visual Studio Code

Vào VS Code -> Extensions (Ctrl + Shift + X) -> Tìm kiếm và cài đặt

- C# Dev Kit
- C#
- NuGet Gallery
- SQLite Viewer

---

## Cài đặt .NET MAUI

Kiểm tra Workload:

```bash
dotnet workload list
```

Kết quả cần có:

```text
maui
```

Nếu chưa có MAUI -> Chạy lệnh:

```bash
dotnet workload install maui
```

---

# 6. Chạy dự án

## Lần đầu clone dự án

Khôi phục package:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

Chạy Android:

```bash
dotnet build -t:Run -f net10.0-android
```

Chạy ios:

```bash
dotnet build -t:Run -f net10.0-ios
```