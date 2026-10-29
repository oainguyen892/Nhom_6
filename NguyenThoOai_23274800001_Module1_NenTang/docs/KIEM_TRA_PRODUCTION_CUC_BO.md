# Kiểm tra Production cục bộ trong phạm vi chạy Visual Studio

Người được giao tiếp quản cấu hình và regression: Nguyễn Thọ Oai — 23274800001. Đặng Tùng Dương — 23274800005 kiểm lại phòng/thống kê; Nguyễn Văn Phong — 23203100062 kiểm lại booking/nhận trả. Theo thông tin người dùng, bài chạy bằng Visual Studio, chưa có yêu cầu hosting/tên miền.

Đã thực thi 4 test ProductionSmokeTests trên TestHost và SQL Server LocalDB riêng. Đây là kiểm thử môi trường Production cục bộ, không phải website được triển khai Internet hoặc chứng chỉ TLS thật. Host `hotel.test` chỉ dùng trong HttpClient/TestServer nội bộ; không truy cập dịch vụ bên ngoài.

| Ca kiểm thử | Kết quả đã chạy |
|---|---|
| Production không tự migrate/seed kể cả Demo:Seed=true | PASS — database chưa tạo khi startup; không có tài khoản demo trên trang chủ |
| HTTP chuyển HTTPS, HTTPS có HSTS | PASS — HTTP 307 và header Strict-Transport-Security trên host thử nghiệm |
| Cookie an toàn và login/admin guard/antiforgery | PASS — cookie antiforgery, Session, TempData có Secure; Session HttpOnly; admin guard và POST thiếu token đúng trạng thái |
| Lỗi DB trả trang lỗi chung | PASS — HTTP 500, không đưa SqlException, stack trace hoặc tên DB test ra HTML |

Lệnh đã chạy:

```powershell
dotnet test -c Release --filter FullyQualifiedName~ProductionSmokeTests --logger 'trx;LogFileName=production-local.trx' --results-directory docs/verification
```

Log: verification/production-local.txt và production-local.trx. Chạy đầu tiên phát hiện cookie TempData chưa có Secure khi test kiểm mọi Set-Cookie; đã cấu hình SecurePolicy.Always ngoài Development cho Session, antiforgery và TempData. Chạy lại đạt 4/4. Development dùng SameAsRequest để chạy profile http của Visual Studio.

Oai phụ trách giữ cấu hình này khi tích hợp. Dương/Phong không tự bỏ cookie Secure, HSTS hoặc antiforgery để chạy Production qua HTTP; khi chạy bài trên Visual Studio dùng profile Development đúng hướng dẫn. Chưa kiểm máy nhiều instance, IIS public, reverse proxy, DNS, chứng chỉ TLS, firewall hay tải lớn; không đánh dấu các điều kiện đó là đạt.

Tham chiếu kỹ thuật: [ASP.NET Core 10 HTTPS/HSTS](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-10.0), [antiforgery](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-10.0).
