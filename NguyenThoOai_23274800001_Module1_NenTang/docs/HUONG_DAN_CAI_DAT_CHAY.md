# Cài đặt và chạy trên Windows

1. Cài .NET 10 SDK x64; Visual Studio Community 2026 có workload ASP.NET hoặc VS Code C#; SQL Server 2019+ / Express / LocalDB. Git cần cho quản lý source. Không cần cài .NET Framework.
2. Giải nén FullSource vào thư mục mới. Mở solution `.sln` hoặc terminal tại root. `dotnet --info` phải có SDK 10; `dotnet tool restore`; `dotnet restore`; `dotnet build -c Release`.
3. Dùng LocalDB: `SqlLocalDB info`, `SqlLocalDB start MSSQLLocalDB`. Nếu chưa có: `SqlLocalDB create MSSQLLocalDB -s`. appsettings.json dùng Windows Authentication và DB riêng `QuanLyKhachSan_UNETI06_TI17A2ND`.
4. `dotnet ef database update --project .\QuanLyKhachSan_UNETI06_TI17A2ND`. Migration nằm trong Migrations; không dùng EnsureCreated thay migration. `dotnet ef migrations list --project .\QuanLyKhachSan_UNETI06_TI17A2ND` và `dotnet ef migrations has-pending-model-changes --project .\QuanLyKhachSan_UNETI06_TI17A2ND` để đối chiếu.
5. `dotnet run --project .\QuanLyKhachSan_UNETI06_TI17A2ND` dùng launch profile Development và port 5066. Hoặc script `scripts/Start-Demo.ps1`. Seed chỉ chạy khi Development và Demo:Seed=true. Lần sau không tạo dữ liệu trùng; khởi tạo lịch demo chỉ khi chưa có phiếu nào.
6. Đăng nhập admin hoặc khach01, mật khẩu demo ở README. Khách xem phòng và đặt; admin mở Dashboard, nhận/trả trên chi tiết phiếu. Không dùng admin để giả lập khách đặt phòng: cần tài khoản khách có hồ sơ hoạt động.

## Visual Studio

Mở `.sln`, chọn project MVC làm Startup Project, profile `http`, kiểm tra chuỗi kết nối. Dùng Terminal tích hợp chạy lệnh migration bên trên, rồi F5/Ctrl+F5. Chạy Test Explorer sau khi tạo instance test. Trong Package Manager Console có thể dùng `Update-Database` với project MVC; tool CLI đã được pin trong `.config/dotnet-tools.json`.

## SQL Server instance khác

```powershell
$env:ConnectionStrings__Hotel='Server=.\SQLEXPRESS;Database=QuanLyKhachSan_UNETI06_TI17A2ND;Integrated Security=true;TrustServerCertificate=true'
$env:ASPNETCORE_ENVIRONMENT='Development'
dotnet ef database update --project .\QuanLyKhachSan_UNETI06_TI17A2ND
dotnet run --project .\QuanLyKhachSan_UNETI06_TI17A2ND --no-launch-profile -- --urls http://localhost:5066
```

Hoặc sửa appsettings.Development.json cục bộ. Với SQL Authentication, lưu credential trong User Secrets/env, không commit. TrustServerCertificate trong ví dụ chỉ dành cho SQL Server development nội bộ. Môi trường dùng thật dùng chứng chỉ hợp lệ, HTTPS, DB riêng, tắt seed; Session memory phù hợp một app instance, nhiều instance cần distributed session store.

## Kiểm thử SQL Server thật

```powershell
SqlLocalDB create Nhom6DeTai05 -s
dotnet test -c Release
```

Nếu đã tồn tại instance, dùng `SqlLocalDB start Nhom6DeTai05`. Tests mặc định dùng instance này; chúng không dùng connection của ứng dụng. Mỗi test integration tạo DB GUID, migrate, seed, chạy HTTP qua WebApplicationFactory và xóa chính DB test khi hoàn tất.

SQL Server khác:

```powershell
$env:HOTEL_TEST_CONNECTION='Server=.\SQLEXPRESS;Database={database};Integrated Security=true;TrustServerCertificate=true'
dotnet test -c Release
```

Bắt buộc giữ `{database}`. Test từ chối connection không tạo tên DB `Nhom6_Test_...`. Tài khoản SQL cần quyền tạo/xóa DB test. Nếu máy chưa có SQL Server, chỉ unit chạy được; không được ghi integration pass. Hệ thống đã chạy integration SQL Server thật trên máy bàn giao, không dùng EF InMemory/SQLite để thay thế.

## Reset và lỗi thường gặp

Dừng app; kiểm tra `ConnectionStrings__Hotel`, tên DB đang trỏ; chạy `dotnet ef database drop --project ...` không thêm `--force`, xác nhận sau khi kiểm tra tên; migrate và chạy Development để seed. Không gọi drop trên dữ liệu dùng thật.

SQL error 26/40: sai instance hoặc chưa start; login failed: thiếu quyền Windows/SQL. NuGet lỗi mạng: kiểm tra api.nuget.org, `dotnet nuget list source`. Model khác migration: chỉ tạo migration mới sau khi cả nhóm thống nhất Entity, không xóa migration đã triển khai. File đang bị khóa khi build: dừng app/debug. Port khác: dùng `--no-launch-profile -- --urls http://localhost:5067`. Tiếng Việt: giữ file UTF-8 và HTML charset utf-8.
