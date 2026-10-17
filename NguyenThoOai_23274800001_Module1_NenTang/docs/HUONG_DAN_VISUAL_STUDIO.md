# Chạy và review bằng Visual Studio

Máy sử dụng Visual Studio, không cần hosting hoặc tên miền để làm bài tập. Người chuẩn bị môi trường/tích hợp: Nguyễn Thọ Oai — 23274800001. Dương (23274800005) chạy phần phòng/thống kê; Phong (23203100062) chạy phần khách/booking/nhận trả.

Đã kiểm kê cài đặt bằng vswhere: Visual Studio Community 2026, installationVersion 18.10.12224.181, trạng thái hoàn tất/có thể khởi chạy. Bằng chứng ở verification/visual-studio-installed.txt. Build/test thực tế chạy bằng .NET CLI; chưa tuyên bố đã thao tác F5 trong giao diện IDE.

1. Mở `QuanLyKhachSan_UNETI06_TI17A2ND.sln` bằng Visual Studio Community 2026, có workload ASP.NET và .NET 10 SDK. Chuột phải project MVC → Set as Startup Project; chọn profile `http`.

Nếu bên phải đang ghi **Solution Explorer - Folder View**, dùng **File → Open → Project/Solution** rồi chọn chính file `.sln` tại thư mục gốc. Sau khi mở đúng, Solution Explorer phải có solution và hai project MVC/tests; file C# không còn nằm dưới Miscellaneous Files. Thanh chạy chọn profile `http`; không chọn chạy trực tiếp file EXE trong bin. Profile đã bật tự mở trình duyệt.
2. Mở View → Terminal tại thư mục solution. `dotnet --info`, `dotnet tool restore`, `dotnet restore --locked-mode`.
3. Chuỗi mặc định dùng `(localdb)\MSSQLLocalDB` và DB riêng QuanLyKhachSan_UNETI06_TI17A2ND. Kiểm `SqlLocalDB info`; nếu cần `SqlLocalDB start MSSQLLocalDB`. Có thể sửa ConnectionStrings:Hotel trong appsettings.Development.json cục bộ, không ghi credential riêng vào Git.
4. Chạy `dotnet ef database update --project .\QuanLyKhachSan_UNETI06_TI17A2ND`. F5 hoặc Ctrl+F5; mở http://localhost:5066. Profile đã đặt Development, seed chỉ chạy Development. Tài khoản demo trong README.
5. Oai kiểm tài khoản/loại phòng/Controller guard; Dương kiểm CRUD phòng/filter/sort/page và số liệu; Phong kiểm hồ sơ/booking/ownership và nhận/trả. Đọc header mỗi file để xác định người được giao phụ trách.
6. Chạy test integration: một lần `SqlLocalDB create Nhom6DeTai05 -s`, hoặc start nếu đã có; mở Test → Test Explorer → Run All. Lệnh tương đương `dotnet test -c Release`. Tests tạo DB GUID riêng; không chọn DB cá nhân để làm test.
7. Cả nhóm mở raw TRX/kết quả tại docs/verification; sau khi sửa mã phải chạy lại. Không coi kết quả có sẵn là bằng chứng người đó đã tự kiểm trên máy mình.

## Khi cần kiểm cấu hình Production ngay trên máy

Chạy `dotnet test -c Release --filter FullyQualifiedName~ProductionSmokeTests`. TestHost mô phỏng HTTPS và môi trường Production trên máy, dùng DB SQL Server test riêng; không triển khai website công khai. Phạm vi và kết quả ở KIEM_TRA_PRODUCTION_CUC_BO.md.

Khi chỉ chạy/bảo vệ bằng Visual Studio, giữ profile Development. Không đổi profile sang Production rồi dùng tài khoản demo để giả lập một triển khai thật. Kiểm tra IIS/server/chứng chỉ Internet không nằm trong phạm vi đã thực hiện.

## Nhận biết chạy sai cấu hình

Nếu Output ghi `Now listening on: http://localhost:5000`, `Hosting environment: Production` và `Content root path: .../bin/Debug/net10.0`, Visual Studio đang chạy chương trình từ thư mục output mà chưa dùng profile project. Có thể xuất hiện `The WebRootPath was not found` vì wwwroot nằm ở thư mục project. Dừng debug, mở file .sln theo các bước trên rồi chọn profile http. Kết quả đúng phải là port 5066, Development và content root là thư mục chứa csproj, không phải bin.
